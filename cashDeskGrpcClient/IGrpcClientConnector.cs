using static GRPC_Client.ProductScannedDTO;
using static GRPC_SaleStoreClient.SaleStoreDTO;

namespace cashDeskGrpcClient
{
    public interface IGrpcClientConnector
    {

        void connect();
        ProductScannedDTOClient getProductScannedDTOClient();

        SaleStoreDTOClient getSaleStoreDTOClient();
    }
}
