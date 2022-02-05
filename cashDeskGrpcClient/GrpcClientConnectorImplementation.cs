using Grpc.Net.Client;
using GRPC_Client;
using GRPC_SaleStoreClient;
using static GRPC_Client.ProductScannedDTO;
using static GRPC_SaleStoreClient.SaleStoreDTO;

namespace cashDeskGrpcClient
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {

        private ProductScannedDTOClient productCDSDTOClient;
        private SaleStoreDTOClient saleStoreDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7134");
            this.productCDSDTOClient = new ProductScannedDTO.ProductScannedDTOClient(channel);
            this.saleStoreDTOClient = new SaleStoreDTO.SaleStoreDTOClient(channel);
        }

        public ProductScannedDTOClient getProductScannedDTOClient()
        {
            return productCDSDTOClient;
        }

        public SaleStoreDTOClient getSaleStoreDTOClient()
        {
            return saleStoreDTOClient;
        }
    }
}