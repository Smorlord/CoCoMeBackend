using static GRPC_Client.ProductScannedDTO;
using static GRPC_PurchaseStoreClient.PurchaseStoreDTO;

namespace cashDeskGrpcClient
{
    public interface IGrpcClientConnector
    {

        void connect();
        ProductScannedDTOClient getProductScannedDTOClient();

        PurchaseStoreDTOClient getPurchaseStoreDTOClient();
    }
}
