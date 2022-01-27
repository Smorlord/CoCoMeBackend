using static GRPC_Client.ProductScannedDTO;

namespace cashDeskGrpcClient
{
    public interface IGrpcClientConnector
    {

        void connect();
        ProductScannedDTOClient getProductScannedDTOClient();
    }
}
