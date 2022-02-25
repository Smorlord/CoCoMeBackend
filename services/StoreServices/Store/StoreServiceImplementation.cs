using data;
using data.EnterpriseData;
using data.StoreData;
using Microsoft.EntityFrameworkCore;
using services.EnterpriseServices;

namespace services.StoreServices
{
    public class StoreServiceImplementation : IStoreService
    {
        private IProductService productService;

        public StoreServiceImplementation(IProductService productService)
        {
            this.productService = productService;
        }

        public void init()
        {
            using (var context = new TradingsystemDbContext())
            {
                if (getStores(context).Count == 0)
                {
                    Store store1 = new Store();
                    store1.Name = "Edeka Nolte";
                    store1.Location = 6; // PLZ-Gebiete 0-9
                    addStore(context, store1);

                    Store store2 = new Store();
                    store2.Name = "Edeka Frise";
                    store2.Location = 6; // PLZ-Gebiete 0-9
                    addStore(context, store2);

                    Store store3 = new Store();
                    store3.Name = "Edeka Feinland";
                    store3.Location = 5; // PLZ-Gebiete 0-9
                    addStore(context, store3);

                    Store store4 = new Store();
                    store4.Name = "Edeka Nobertson";
                    store4.Location = 6; // PLZ-Gebiete 0-9
                    addStore(context, store4);
                }
            }

            using (var context = new TradingsystemDbContext())
            {
                if (getStockItemByStore(context, 1).Count == 0)
                {
                    // Store 1

                    StockItem stockItem1 = new StockItem();
                    stockItem1.MinStock = 100;
                    stockItem1.Amount = 103;
                    stockItem1.MaxStock = 200;
                    stockItem1.Product = productService.getProductByBarcode(context, 1111);
                    addStockItemByStore(context, 1, stockItem1);

                    StockItem stockItem2 = new StockItem();
                    stockItem2.MinStock = 130;
                    stockItem2.Amount = 215;
                    stockItem2.MaxStock = 250;
                    stockItem2.Product = productService.getProductByBarcode(context, 2222);
                    addStockItemByStore(context, 1, stockItem2);

                    StockItem stockItem3 = new StockItem();
                    stockItem3.MinStock = 80;
                    stockItem3.Amount = 143;
                    stockItem3.MaxStock = 170;
                    stockItem3.Product = productService.getProductByBarcode(context, 3333);
                    addStockItemByStore(context, 1, stockItem3);
                }

                if (getStockItemByStore(context, 2).Count == 0)
                {
                    // Store 2

                    StockItem stockItem11 = new StockItem();
                    stockItem11.MinStock = 100;
                    stockItem11.Amount = 200;
                    stockItem11.MaxStock = 200;
                    stockItem11.Product = productService.getProductByBarcode(context, 1111);
                    addStockItemByStore(context, 2, stockItem11);

                    StockItem stockItem22 = new StockItem();
                    stockItem22.MinStock = 130;
                    stockItem22.Amount = 233;
                    stockItem22.MaxStock = 250;
                    stockItem22.Product = productService.getProductByBarcode(context, 2222);
                    addStockItemByStore(context, 2, stockItem22);
                }

                if (getStockItemByStore(context, 3).Count == 0)
                {
                    // Store 3

                    StockItem stockItem111 = new StockItem();
                    stockItem111.MinStock = 100;
                    stockItem111.Amount = 103;
                    stockItem111.MaxStock = 200;
                    stockItem111.Product = productService.getProductByBarcode(context, 1111);
                    addStockItemByStore(context, 3, stockItem111);
                }

                if (getStockItemByStore(context, 4).Count == 0)
                {
                    // Store 4

                    StockItem stockItem1111 = new StockItem();
                    stockItem1111.MinStock = 100;
                    stockItem1111.Amount = 199;
                    stockItem1111.MaxStock = 200;
                    stockItem1111.Product = productService.getProductByBarcode(context, 1111);
                    addStockItemByStore(context, 4, stockItem1111);

                    StockItem stockItem2222 = new StockItem();
                    stockItem2222.MinStock = 130;
                    stockItem2222.Amount = 215;
                    stockItem2222.MaxStock = 250;
                    stockItem2222.Product = productService.getProductByBarcode(context, 2222);
                    addStockItemByStore(context, 4, stockItem2222);
                }
            }

            using (var context = new TradingsystemDbContext())
            {
                if (getProductSales(context, 1).Count == 0)
                {
                    updateProductSale(context, 1, productService.getProductByBarcode(context, 1111).Id, 0.99);
                    context.SaveChanges();
                }
            }
        }

        public void addStore(TradingsystemDbContext context, Store Store)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Add(Store);
                db.SaveChanges();
            }
        }

        public Store getStore(TradingsystemDbContext context, int StoreId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Stores
                    .Include(s => s.ProductSales)
                    .Include(s => s.StockItems).ThenInclude(s => s.Product)
                    .Include(s => s.Sales).ThenInclude(p => p.PurchaseItems)
                    .Include(s => s.ExchangeEntry)
                    .FirstOrDefault(p => p.Id == StoreId);
            }
        }

        public List<Store> getStores(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Stores != null ? db.Stores.ToList() : new List<Store>();
            }
        }

        public void updateStockItemsInStore(TradingsystemDbContext context, int storeId, List<OrderEntry> entries)
        {
            using var db = TradingsystemDbContext.GetContext(context);
            Store store = getStore(db, storeId);
            store.StockItems.ForEach(i =>
            {
                var entry = entries.Find(e => e.ProductId == i.Product.Id);
                i.Amount += (entry?.Amount ?? 0);
            });
            db.SaveChanges();
        }

        public ProductSale getProductSaleByProductId(TradingsystemDbContext context, int StoreId, int ProductId)
        {
            foreach (var productSale in getProductSales(context, StoreId))
            {
                if (productSale.Product.Id == ProductId)
                {
                    return productSale;
                }
            }

            return null;
        }

        public List<ProductSale> getProductSales(TradingsystemDbContext context, int StoreId)
        {
            Store store = getStore(context, StoreId);
            return store.ProductSales.ToList();
        }

        public void updateProductSale(TradingsystemDbContext context, int StoreId, int ProductId, double SalesPrice)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Store store = getStore(db, StoreId);
                ProductSale? existingProductSale = store.ProductSales.Find(p => p.ProductId == ProductId);
                if (existingProductSale is not null)
                {
                    existingProductSale.SalePrice = SalesPrice;
                }
                else
                {
                    ProductSale productSale = new ProductSale
                    {
                        SalePrice = SalesPrice, Product = productService.getProduct(db, ProductId),
                        ProductId = ProductId
                    };
                    store.ProductSales.Add(productSale);
                }

                db.SaveChanges();
            }
        }

        public void removeProductSale(TradingsystemDbContext context, int StoreId, int ProductSaleId)
        {
            throw new NotImplementedException();
        }

        public ProductSale getProductSaleById(TradingsystemDbContext context, int ProductSaleId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                ProductSale productSale =
                    db.ProductSales.Include(p => p.Product).FirstOrDefault(p => p.Id == ProductSaleId);
                return productSale;
            }
        }


        public List<StockItem> getStockItemByStore(TradingsystemDbContext context, int StoreId)
        {
            Store store = getStore(context, StoreId);
            return store.StockItems.ToList();
        }

        public StockItem? getStockItemByStoreByProduct(TradingsystemDbContext context, int StoreId, int ProductId)
        {
            Store store = getStore(context, StoreId);
            return store.StockItems.Find(i => i.Product.Id == ProductId);
        }

        public void addStockItemByStore(TradingsystemDbContext context, int StoreId, StockItem StockItem)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Store store = getStore(db, StoreId);
                store.StockItems.Add(StockItem);
                db.SaveChanges();
            }
        }

        public void changeExchangeStatusInStockItem (int storeId)
        {
            using (var db = new TradingsystemDbContext())
            {
                Store reciever = getStore(db, storeId);
                ExchangeEntry deleteEntry = null;

                reciever.StockItems.ForEach(recieverItem =>
                {
                    reciever.ExchangeEntry.ForEach(exchangeEntry =>
                    {
                        if (recieverItem.Product.Id == exchangeEntry.Product.Id)
                        {
                            recieverItem.Amount += exchangeEntry.ExchangeAmount;
                            recieverItem.ExchangeStatus = null;
                            deleteEntry = exchangeEntry;

                            getStore(db, exchangeEntry.SupplierStoreId).StockItems.ForEach(supplierItem =>
                            {
                                if (supplierItem.Product.Id == exchangeEntry.Product.Id)
                                {
                                    supplierItem.ExchangeStatus = null;
                                }
                            });
                        }
                    });
                    if(deleteEntry != null) { 
                        reciever.ExchangeEntry.Remove(deleteEntry);
                    }

                });

                db.SaveChanges();
            }
        }
    }
}