using Microsoft.Extensions.Logging;
using services.StoreServices;
using Grpc.Core;
using data.StoreData;
using GRPC_SaleEnterpriseServer;

namespace GRPC_Service.Services
{
    public class SaleEnterpriseGrpcService : SaleEnterpriseDTO.SaleEnterpriseDTOBase
    {
        private readonly ILogger<SaleEnterpriseGrpcService> _logger;
        private ISaleService service;

        public SaleEnterpriseGrpcService(ILogger<SaleEnterpriseGrpcService> logger, ISaleService saleService)
        {
            this.service = saleService;
            this._logger = logger;
        }

        public override Task<CreateSaleEnterpriseDTOModel> CreateSaleEnterprise(CreateSaleEnterpriseDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                CreateSaleEnterpriseDTOModel output = new CreateSaleEnterpriseDTOModel();

                Sale sale = service.createSale(request.StoreId);
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
    }
}
