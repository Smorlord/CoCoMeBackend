using Grpc.Net.Client;
using GRPC_Client;
using GRPC_PurchaseEnterpriseClient;
using static GRPC_Client.ProductDTO;
using static GRPC_PurchaseEnterpriseClient.PurchaseEnterpriseDTO;

namespace grpcClientStore
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {
        private ProductDTOClient productCDSDTOClient;
        private PurchaseEnterpriseDTOClient saleDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7244");
            this.productCDSDTOClient = new ProductDTO.ProductDTOClient(channel);
            this.saleDTOClient = new PurchaseEnterpriseDTO.PurchaseEnterpriseDTOClient(channel);
        }

        public ProductDTOClient GetProductDTOClient()
        {
           return this.productCDSDTOClient;
        }

        public PurchaseEnterpriseDTO.PurchaseEnterpriseDTOClient GetPurchaseEnterpriseDTOClient()
        {
            return this.saleDTOClient;
        }
    }
}
