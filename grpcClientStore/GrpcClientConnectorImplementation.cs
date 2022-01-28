using Grpc.Net.Client;
using GRPC_Client;
using static GRPC_Client.ProductDTO;

namespace grpcClientStore
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {
        private ProductDTOClient productCDSDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7244");
            this.productCDSDTOClient = new ProductDTO.ProductDTOClient(channel);
        }

        public ProductDTOClient GetProductDTOClient()
        {
           return this.productCDSDTOClient;
        }
    }
}
