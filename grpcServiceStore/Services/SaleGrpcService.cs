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

        public override Task<UpdateSaleStoreDTOModel> UpdateSaleStore(UpdateSaleStoreDTOLookUpModel request, ServerCallContext context)
        {
            try
            {
                // Entpacken der Store DTOS und in Enterprise DTOS mappen
                UpdateSaleStoreDTOModel output = new UpdateSaleStoreDTOModel();
                List<ProductEnterpriseDTOLookUpModel> productSalesEnterprise = new List<ProductEnterpriseDTOLookUpModel>();
                foreach (var storeProduct in request.ProductStoreDTOLookUpModel)
                {
                    ProductEnterpriseDTOLookUpModel product = new ProductEnterpriseDTOLookUpModel();
                    product.Id = storeProduct.Id;

                    productSalesEnterprise.Add(product);
                }
                UpdateSaleEnterpriseDTOLookUpModel data = new UpdateSaleEnterpriseDTOLookUpModel();
                data.SaleId = request.SaleId;
                data.ProductEnterpriseDTOLookUpModel.AddRange(productSalesEnterprise);

                //Weiter reichen der Daten an den Enterprise Server
                UpdateSaleEnterpriseDTOModel responseUpdate = this.grpcClientConnector.GetSaleEnterpriseDTOClient().UpdateSaleEnterprise(data);


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
                output.SaleId = responseUpdate.SaleId;
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

