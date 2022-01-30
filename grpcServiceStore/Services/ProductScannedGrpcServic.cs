using Grpc.Core;
using GRPC_Client;
using GRPC_Serer;
using grpcClientStore;
using Microsoft.Extensions.Logging;
using services.EnterpriseServices;

namespace grpcServiceStore.Services
{
    public class ProductScannedGrpcService : ProductScannedDTO.ProductScannedDTOBase
    {
        private readonly ILogger<ProductScannedGrpcService> _logger;
        private IGrpcClientConnector grpcClientConnector;

        public ProductScannedGrpcService(ILogger<ProductScannedGrpcService> logger, IGrpcClientConnector grpcClientConnector)
        {
            this._logger = logger;
            this.grpcClientConnector = grpcClientConnector;
        }

        public override Task<ProductScannedDTOModel> GetProductScannedDTOInfo(ProductScannedDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                ProductScannedDTOModel output = new ProductScannedDTOModel();
                
                ProductDTOModel response = grpcClientConnector.GetProductDTOClient().GetProductDTOInfo(new ProductDTOLookUpModel{ Barcode = request.Barcode });

                output.Id = response.Id;
                output.Barcode = response.Barcode;
                output.Name = response.Name;
                output.PurchasePrice = response.PurchasePrice;

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
