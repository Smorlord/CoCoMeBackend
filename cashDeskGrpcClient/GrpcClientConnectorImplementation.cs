using Grpc.Net.Client;
using GRPC_Client;
using static GRPC_Client.ProductScannedDTO;

namespace cashDeskGrpcClient
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {

        private ProductScannedDTOClient productCDSDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7244");
            this.productCDSDTOClient = new ProductScannedDTO.ProductScannedDTOClient(channel);
        }

        public ProductScannedDTOClient getProductScannedDTOClient()
        {
            return productCDSDTOClient;
        }
    }
}