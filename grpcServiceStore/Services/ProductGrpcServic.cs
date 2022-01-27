using data.EnterpriseData;
using Grpc.Core;
using GRPC_Service;
using Microsoft.Extensions.Logging;
using services.EnterpriseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grpcServiceStore.Services
{
    public class ProductGrpcService : ProductScannedDTO.ProductScannedDTOBase
    {
        private readonly ILogger<ProductGrpcService> _logger;
        private ProductService service;

        public ProductGrpcService(ILogger<ProductGrpcService> logger, ProductService productService)
        {
            this.service = productService;
            this._logger = logger;
        }

        public override Task<ProductScannedDTOModel> GetProductScannedDTOInfo(ProductScannedDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                ProductScannedDTOModel output = new ProductScannedDTOModel();

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
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }
    }
}
