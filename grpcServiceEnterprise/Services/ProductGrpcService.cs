using Grpc.Core;
using services.EnterpriseServices;
using data.EnterpriseData;
using Microsoft.Extensions.Logging;
/**
* Service für Customer
*/

namespace GRPC_Service.Services
{
    public class ProductGrpcService : ProductDTO.ProductDTOBase
    {
        private readonly ILogger<ProductGrpcService> _logger;
        private ProductService service;

        public ProductGrpcService(ILogger<ProductGrpcService> logger, ProductService productService)
        {
            this.service = productService;
            this._logger = logger;
        }

        public override Task<ProductDTOModel> GetProductDTOInfo(ProductDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                ProductDTOModel output = new ProductDTOModel();

                Product product = service.getProductByBarcode(request.Barcode);
                if (product == null)
                {
                    // TODO throw RPC Exception NoValueFound
                    return null;
                }

                output.Id = product.Id;
                output.Barcode = product.Barcode;
                output.Name = product.Name;
                output.PurchasePrice = product.PurchasePrice;

                return Task.FromResult(output);
            } catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }
    }
}
