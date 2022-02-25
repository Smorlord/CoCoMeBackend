using Microsoft.Extensions.Logging;
using services.StoreServices;
using Grpc.Core;
using data.StoreData;
using GRPC_PurchaseEnterpriseServer;
using data.EnterpriseData;
using services.EnterpriseServices;
using data;

namespace GRPC_Service.Services
{
    public class PurchaseEnterpriseGrpcService : PurchaseEnterpriseDTO.PurchaseEnterpriseDTOBase
    {
        private readonly ILogger<PurchaseEnterpriseGrpcService> _logger;
        private IPurchaseService saleService;
        private IProductService productService;
        private IStoreService storeService;

        public PurchaseEnterpriseGrpcService(ILogger<PurchaseEnterpriseGrpcService> logger, IPurchaseService saleService, IProductService productService, IStoreService storeService)
        {
            this.saleService = saleService;
            this._logger = logger;
            this.productService = productService;
            this.storeService = storeService;
        }

        public override Task<CreatePurchaseEnterpriseDTOModel> CreatePurchaseEnterprise(CreatePurchaseEnterpriseDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                CreatePurchaseEnterpriseDTOModel output = new CreatePurchaseEnterpriseDTOModel();

                Purchase Purchase = saleService.createPurchase(null, request.StoreId);
                if (Purchase == null)
                {
                    // TODO throw RPC Exception NoValueFound
                    return null;
                }

                output.PurchaseId = Purchase.Id;

                return Task.FromResult(output);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }

        public override Task<UpdatePurchaseEnterpriseDTOModel> UpdatePurchaseEnterprise(UpdatePurchaseEnterpriseDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                UpdatePurchaseEnterpriseDTOModel output = new UpdatePurchaseEnterpriseDTOModel();
                List<PurchaseItem> purchaseItems = new List<PurchaseItem>();
                Purchase PurchaseResponse;
                using (var db = new TradingsystemDbContext())
                {
                    foreach (var productDTO in request.ProductEnterpriseDTOLookUpModel)
                    {
                        Purchase purchase = saleService.getPurchaseById(db, request.PurchaseId);
                        PurchaseItem purchaseItem = new PurchaseItem();
                        ProductSale productSale = storeService.getProductSaleByProductId(db, purchase.StoreId, productDTO.Id);
                        Product product = productService.getProduct(db, productDTO.Id);
                        if (productSale != null)
                        {
                            purchaseItem.PurchasePrice = productSale.SalePrice;
                        }
                        purchaseItem.Product = product;
                        purchaseItems.Add(purchaseItem);

                    }
                    PurchaseResponse = saleService.updatePurchase(db, request.PurchaseId, purchaseItems);

                    List<ProductEnterpriseDTOModel> productSalesEnterprise = new List<ProductEnterpriseDTOModel>();
                    foreach (var purchaseItem in PurchaseResponse.PurchaseItems)
                    {
                        ProductEnterpriseDTOModel product = new ProductEnterpriseDTOModel();
                        product.Name = purchaseItem.Product.Name;
                        product.Id = purchaseItem.Product.Id;
                        product.PurchasePrice = purchaseItem.Product.SellingPrice;
                        product.Barcode = purchaseItem.Product.Barcode;

                        ProductSale productSale = storeService.getProductSaleByProductId(db, PurchaseResponse.Store.Id, product.Id);
                        if (productSale != null)
                        {
                            product.SalePrice = productSale.SalePrice;
                        } 
                        else
                        {
                            product.SalePrice = -1;
                        }


                        /** 
                         * Aus den Stockitems des Stores muss das Stockitem gefunden werden und die Anzahl um eins reduziert werden 
                         */
                        storeService.getStore(db, PurchaseResponse.Store.Id).StockItems.ForEach( item =>
                        {
                            if (item.Product.Id == purchaseItem.Product.Id && item.Amount > item.MinStock)
                            {
                                item.Amount--; // reduziert Anzahl der Produkte um eins, wenn es gekauft wurde
                            }
                        });
                        db.SaveChanges();


                        /** 
                         * UC 8 Product Exchange (on low stock) Among Stores 
                         * Prüfe Stockamount
                         */
                        Store store = storeService.getStore(db, PurchaseResponse.Store.Id);
                        store.StockItems.ForEach(async stockItem => 
                        {
                            if (stockItem.Amount <= stockItem.MinStock && !stockItem.IsExpected)
                            {
                                Console.WriteLine($"UC 8 - Start - Das Produkt {stockItem.Product.Name} mit der Anzahl {stockItem.Amount} ist zu gering");
                                // should run async without waiting for something with .ConfigureAwait(false)
                                await ProductExchange(db, store.Id, store.Location, stockItem).ConfigureAwait(false);
                            }
                        });


                        productSalesEnterprise.Add(product);
                    }
                    output.ProductEnterpriseDTOModel.AddRange(productSalesEnterprise);
                }
                
                output.PurchaseId = PurchaseResponse.Id;

                return Task.FromResult(output);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }

        /**
         * Eigentlich sollte ein StoreServer der weiß, dass einer seiner Stores ein Produkt benötigt, eigenständig den EnterpriseServer 
         * anstoßen, mit dem gewünschten Produkt und der EnterpriseServer sendet eine Anfrage an die StoreServer die in der Nähe des 
         * Store liegen. Die StoreServer durchsuchen die eigenen Produkte falls genug vorrätig, wird das Produkt als reserviert markiert 
         * und an den EnterpriseServer kommuniziert. Der EnterpriseServer verarbeitet das dahingehen, dass die beste Option genommen wird 
         * und die Lieferung veranlasst wird. Der Status wird angepasst sobald der Tranfer durchgeführt ist.
         * Der Anwendungsfall wird Server intern geregelt, es sind keine "Menschen" beteiligt.
         */
        public async Task ProductExchange(TradingsystemDbContext db, int storeId, int storeLocation, StockItem emptyStockItem)
        {
            /** Liste mit Store die genügend amount eines Produktes haben -> sortieren nach meiste zu erste
             * dann einfach erste Store (meiste anzahl an items) nehmen und eine Anfrage  (weiterer Verlauf überlegen)
             */
            List<ExchangeEntry> exchangeEntrys = new List<ExchangeEntry>();

            storeService.getStores(db).ForEach( exStore => { // get all Stores

                if (exStore.Location == storeLocation) // PLZ-Gebiete 0-9 wenn im Selben liefergebiet
                {
                    exStore.StockItems.ForEach( (stockItem) => { // get all StockItems from this store

                        // prüfe Stockitem ist gleiche wie gesuchtes StockItem && SockitemAmount ist größer als MinStock && stockitem.Amaount des angefragten Stores ist größer als benötigete Anzahl des EmtyStock 
                        if (emptyStockItem.Product.Id == stockItem.Product.Id && stockItem.Amount > stockItem.MinStock  && (emptyStockItem.MinStock - emptyStockItem.Amount) < (stockItem.Amount - stockItem.MinStock))
                        {
                            // Objekt mit Storeid, Produkt und verfügbare Anzahl der Produkte die angegeben werden können
                            ExchangeEntry exchangeEntry = new ExchangeEntry();

                            exchangeEntry.ExchangeAmount = (stockItem.Amount - stockItem.MinStock);
                            exchangeEntry.StoreId = exStore.Id;
                            exchangeEntry.Product = stockItem.Product;
                            exchangeEntrys.Add(exchangeEntry);
                        }
                    });
                }
            });
            db.SaveChanges();

            /** 
             * Hier würden Heuristiken zum Einsatz kommen. Wir nehmen eine sortierte Liste von Anzahl 
             * hoch -> niedirg und davon das erste Element (höchste Anzahl an Produkten über dem MinStock)
             */
            exchangeEntrys.Sort( (y, x) => x.ExchangeAmount.CompareTo(y.ExchangeAmount) ); // ascending order 
            ExchangeEntry entry = exchangeEntrys[0]; // mit der höchsten Anzahl an lieferbaren Produkten

            // Zulieferer "unavailable";
            StockItem supplier = storeService.getStore(db, entry.StoreId).StockItems.Find(stockitem => stockitem.Product.Id == entry.Product.Id);
            supplier.ExchangeStatus = ExchangeStatus.Unavailable;
            // Empfänger "incoming";
            StockItem receiver = storeService.getStore(db, storeId).StockItems.Find(stockitem => stockitem.Product.Id == emptyStockItem.Product.Id);
            receiver.ExchangeStatus = ExchangeStatus.Incoming;

            entry.ExchangeAmount = entry.ExchangeAmount / 2; // TODO speichere den wert von emtyStockItem.Amount - emtyStockItem.MinStock


            /**
             * Hier würde der Lieferauftrag erstellt werden mit dem entry.ExchangeAmount
             * Bei Abschluss muss der Store der das Produkt zum Store liefert, dass das Produkt benötgt
             * sein eigenes Inventar updaten
             */


            // Store mit dem benötigeten Produkt bekommt einen Status zugesendet (hier wird er einfach gesetzt)...
            Store store = storeService.getStore(db, storeId);
            store.ExchangeEntry.Add(entry); // enthält StoreId von dem die Lieferung stammt, das Produkt und die Anzahl der Einheiten
            db.SaveChanges();



        }
    }
}
