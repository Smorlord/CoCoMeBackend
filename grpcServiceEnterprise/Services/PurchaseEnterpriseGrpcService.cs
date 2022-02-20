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
                        } else
                        {
                            product.SalePrice = -1;
                        }


                        // Aus den Stockitems des Stores muss das Stockitem gefunden werden und die Anzahl um eins reduziert werden
                        storeService.getStore(db, PurchaseResponse.Store.Id).StockItems.ForEach(item =>
                        {
                            if(item.Product.Id == purchaseItem.Product.Id)
                            {
                                item.Amount--;
                            }
                        });
                        db.SaveChanges();


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
    }
}
