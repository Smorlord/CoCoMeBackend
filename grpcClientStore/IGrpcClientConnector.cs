using static GRPC_Client.ProductDTO;
using static GRPC_SaleEnterpriseClient.SaleEnterpriseDTO;

namespace grpcClientStore
{
    public interface IGrpcClientConnector
    {
        void connect();
        ProductDTOClient GetProductDTOClient();
        SaleEnterpriseDTOClient GetSaleEnterpriseDTOClient();
    }
}