using Grpc.Core;
using services.EnterpriseServices;
using data.EnterpriseData;
using Microsoft.Extensions.Logging;
using GRPC_Server;
using data.StoreData;
using services.StoreServices;
using data;
/**
* Service für Customer
*/

namespace GRPC_Service.Services
{
    public class ProductGrpcService : ProductDTO.ProductDTOBase
    {
        private readonly ILogger<ProductGrpcService> _logger;
        private IProductService service;
        private IStoreService storeService;

        public ProductGrpcService(ILogger<ProductGrpcService> logger, IProductService productService, IStoreService storeService)
        {
            this.service = productService;
            this._logger = logger;
            this.storeService = storeService;
        }

        public override Task<ProductDTOModel> GetProductDTOInfo(ProductDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                ProductDTOModel output = new ProductDTOModel();

                Product product = service.getProductByBarcode(null, request.Barcode);
                if (product == null)
                {
                    // TODO throw RPC Exception NoValueFound
                    return null;
                }
                using (var db = new TradingsystemDbContext()) {
                    ProductSale productSale = storeService.getProductSaleByProductId(db, request.StoreId, product.Id);


                    output.Id = product.Id;
                    output.Barcode = product.Barcode;
                    output.Name = product.Name;
                    output.SellingPrice = product.SellingPrice;

                    if (productSale != null)
                    {
                        output.SalePrice = productSale.SalePrice;
                    }
                    else
                    {
                        output.SalePrice = -1;
                    }
                }

                return Task.FromResult(output);
            } catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }
    }
}
