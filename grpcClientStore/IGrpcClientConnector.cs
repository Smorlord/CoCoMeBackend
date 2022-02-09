using static GRPC_Client.ProductDTO;
using static GRPC_PurchaseEnterpriseClient.PurchaseEnterpriseDTO;

namespace grpcClientStore
{
    public interface IGrpcClientConnector
    {
        void connect();
        ProductDTOClient GetProductDTOClient();
        PurchaseEnterpriseDTOClient GetPurchaseEnterpriseDTOClient();
    }
}