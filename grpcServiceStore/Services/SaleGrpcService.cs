using Microsoft.Extensions.Logging;
using Grpc.Core;
using grpcClientStore;
using GRPC_SaleStoreServer;
using GRPC_SaleEnterpriseClient;

namespace grpcServiceStore.Services
{
    public class SaleGrpcService : SaleStoreDTO.SaleStoreDTOBase
    {
        private readonly ILogger<SaleGrpcService> _logger;
        private IGrpcClientConnector grpcClientConnector;

        public SaleGrpcService(ILogger<SaleGrpcService> logger, IGrpcClientConnector grpcClientConnector)
        {
            this.grpcClientConnector = grpcClientConnector;
            this._logger = logger;
        }

        public override Task<CreateSaleStoreDTOModel> CreateSaleStore(CreateSaleStoreDTOLookUpModel request, ServerCallContext context)
        {
            try
            {

                CreateSaleStoreDTOModel output = new CreateSaleStoreDTOModel();

                CreateSaleEnterpriseDTOModel response = grpcClientConnector.GetSaleEnterpriseDTOClient().CreateSaleEnterprise(new CreateSaleEnterpriseDTOLookUpModel { StoreId = request.StoreId });

                output.SaleId = response.SaleId;

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

