using Grpc.Net.Client;
using GRPC_Client;
using GRPC_PurchaseStoreClient;
using static GRPC_Client.ProductScannedDTO;
using static GRPC_PurchaseStoreClient.PurchaseStoreDTO;

namespace cashDeskGrpcClient
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {

        private ProductScannedDTOClient productCDSDTOClient;
        private PurchaseStoreDTOClient saleStoreDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7134");
            this.productCDSDTOClient = new ProductScannedDTO.ProductScannedDTOClient(channel);
            this.saleStoreDTOClient = new PurchaseStoreDTO.PurchaseStoreDTOClient(channel);
        }

        public ProductScannedDTOClient getProductScannedDTOClient()
        {
            return productCDSDTOClient;
        }

        public PurchaseStoreDTOClient getPurchaseStoreDTOClient()
        {
            return saleStoreDTOClient;
        }
    }
}