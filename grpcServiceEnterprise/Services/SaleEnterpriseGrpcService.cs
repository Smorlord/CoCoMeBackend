using Microsoft.Extensions.Logging;
using services.StoreServices;
using Grpc.Core;
using data.StoreData;
using GRPC_SaleEnterpriseServer;
using data.EnterpriseData;
using services.EnterpriseServices;
using data;

namespace GRPC_Service.Services
{
    public class SaleEnterpriseGrpcService : SaleEnterpriseDTO.SaleEnterpriseDTOBase
    {
        private readonly ILogger<SaleEnterpriseGrpcService> _logger;
        private ISaleService saleService;
        private ProductService productService;
        private IStoreService storeService;

        public SaleEnterpriseGrpcService(ILogger<SaleEnterpriseGrpcService> logger, ISaleService saleService, ProductService productService, IStoreService storeService)
        {
            this.saleService = saleService;
            this._logger = logger;
            this.productService = productService;
            this.storeService = storeService;
        }

        public override Task<CreateSaleEnterpriseDTOModel> CreateSaleEnterprise(CreateSaleEnterpriseDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                CreateSaleEnterpriseDTOModel output = new CreateSaleEnterpriseDTOModel();

                Purchase sale = saleService.createSale(null, request.StoreId);
                if (sale == null)
                {
                    // TODO throw RPC Exception NoValueFound
                    return null;
                }

                output.SaleId = sale.Id;

                return Task.FromResult(output);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }

        public override Task<UpdateSaleEnterpriseDTOModel> UpdateSaleEnterprise(UpdateSaleEnterpriseDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                UpdateSaleEnterpriseDTOModel output = new UpdateSaleEnterpriseDTOModel();
                List<PurchaseItem> purchaseItems = new List<PurchaseItem>();
                Purchase saleResponse;
                using (var db = new TradingsystemDbContext())
                {
                    foreach (var productDTO in request.ProductEnterpriseDTOLookUpModel)
                    {
                        Purchase purchase = saleService.getSaleById(db, request.SaleId);
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
                    saleResponse = saleService.updateSale(db, request.SaleId, purchaseItems);

                    List<ProductEnterpriseDTOModel> productSalesEnterprise = new List<ProductEnterpriseDTOModel>();
                    foreach (var purchaseItem in saleResponse.PurchaseItems)
                    {
                        ProductEnterpriseDTOModel product = new ProductEnterpriseDTOModel();
                        product.Name = purchaseItem.Product.Name;
                        product.Id = purchaseItem.Product.Id;
                        product.PurchasePrice = purchaseItem.Product.SellingPrice;
                        product.Barcode = purchaseItem.Product.Barcode;

                        ProductSale productSale = storeService.getProductSaleByProductId(db, saleResponse.Store.Id, product.Id);
                        if (productSale != null)
                        {
                            product.SalePrice = productSale.SalePrice;
                        } else
                        {
                            product.SalePrice = -1;
                        }

                        productSalesEnterprise.Add(product);
                    }
                    output.ProductEnterpriseDTOModel.AddRange(productSalesEnterprise);
                }
                
                output.SaleId = saleResponse.Id;

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
