using Microsoft.Extensions.Logging;
using Grpc.Core;
using grpcClientStore;
using GRPC_PurchaseStoreServer;
using GRPC_PurchaseEnterpriseClient;

namespace grpcServiceStore.Services
{
    public class PurchaseGrpcService : PurchaseStoreDTO.PurchaseStoreDTOBase
    {
        private readonly ILogger<PurchaseGrpcService> _logger;
        private IGrpcClientConnector grpcClientConnector;

        public PurchaseGrpcService(ILogger<PurchaseGrpcService> logger, IGrpcClientConnector grpcClientConnector)
        {
            this.grpcClientConnector = grpcClientConnector;
            this._logger = logger;
        }

        public override Task<CreatePurchaseStoreDTOModel> CreatePurchaseStore(CreatePurchaseStoreDTOLookUpModel request, ServerCallContext context)
        {
            try
            {

                CreatePurchaseStoreDTOModel output = new CreatePurchaseStoreDTOModel();

                CreatePurchaseEnterpriseDTOModel response = grpcClientConnector.GetPurchaseEnterpriseDTOClient().CreatePurchaseEnterprise(new CreatePurchaseEnterpriseDTOLookUpModel { StoreId = request.StoreId });

                output.PurchaseId = response.PurchaseId;

                return Task.FromResult(output);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                throw new RpcException(Status.DefaultCancelled);
            }
        }

        public override Task<UpdatePurchaseStoreDTOModel> UpdatePurchaseStore(UpdatePurchaseStoreDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                // Entpacken der Store DTOS und in Enterprise DTOS mappen
                UpdatePurchaseStoreDTOModel output = new UpdatePurchaseStoreDTOModel();
                List<ProductEnterpriseDTOLookUpModel> productSalesEnterprise = new List<ProductEnterpriseDTOLookUpModel>();
                foreach (var storeProduct in request.ProductStoreDTOLookUpModel)
                {
                    ProductEnterpriseDTOLookUpModel product = new ProductEnterpriseDTOLookUpModel();
                    product.Id = storeProduct.Id;

                    productSalesEnterprise.Add(product);
                }
                UpdatePurchaseEnterpriseDTOLookUpModel data = new UpdatePurchaseEnterpriseDTOLookUpModel();
                data.PurchaseId = request.PurchaseId;
                data.ProductEnterpriseDTOLookUpModel.AddRange(productSalesEnterprise);

                //Weiter reichen der Daten an den Enterprise Server
                UpdatePurchaseEnterpriseDTOModel responseUpdate = this.grpcClientConnector.GetPurchaseEnterpriseDTOClient().UpdatePurchaseEnterprise(data);


                // Entpacken der Enterprise DTOS und in Store DTOS mappen
                List<ProductStoreDTOModel> productSalesStore = new List<ProductStoreDTOModel>();
                foreach (var enterpriseProduct in responseUpdate.ProductEnterpriseDTOModel)
                {
                    ProductStoreDTOModel product = new ProductStoreDTOModel();
                    product.Name = enterpriseProduct.Name;
                    product.Id = enterpriseProduct.Id;
                    product.PurchasePrice = enterpriseProduct.PurchasePrice;
                    product.Barcode = enterpriseProduct.Barcode;
                    product.SalePrice = enterpriseProduct.SalePrice;

                    productSalesStore.Add(product);

                    //Überprüfen ob SalePrice gesetzt
                    //Gesamtbetrag berechnen
                    if (product.SalePrice == -1)
                    {
                        output.SalePriceTotal += product.PurchasePrice;
                    } else
                    {
                        output.SalePriceTotal += product.SalePrice;
                    }
                }
                output.PurchaseId = responseUpdate.PurchaseId;
                output.ProductStoreDTOModel.AddRange(productSalesStore);

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

