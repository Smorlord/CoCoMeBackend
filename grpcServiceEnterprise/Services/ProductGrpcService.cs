using Grpc.Core;
using services.EnterpriseServices;
using data.EnterpriseData;
/**
 * Service für Customer
 */

namespace GRPC_Service.Services
{
    public class ProductGrpcService : ProductDTO.ProductDTOBase
    {

        private ProductService service;

        public ProductGrpcService(ProductService productService)
        {
            this.service = productService;
        }

        public override Task<ProductDTOModel> GetProductDTOInfo(ProductDTOLookUpModel request, ServerCallContext context)
        {
            ProductDTOModel output = new ProductDTOModel();

            Product product = service.getProductByBarcode(request.Barcode);
            if (product == null)
            {
                return null;
            }

            output.Id = product.Id;
            output.Barcode = product.Barcode;
            output.Name = product.Name;
            output.PurchasePrice = product.PurchasePrice;

            return Task.FromResult(output);
        }
    }
}
