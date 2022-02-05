using Grpc.Net.Client;
using GRPC_Client;
using GRPC_SaleEnterpriseClient;
using static GRPC_Client.ProductDTO;
using static GRPC_SaleEnterpriseClient.SaleEnterpriseDTO;

namespace grpcClientStore
{
    public class GrpcClientConnectorImplementation : IGrpcClientConnector
    {
        private ProductDTOClient productCDSDTOClient;
        private SaleEnterpriseDTOClient saleDTOClient;
        private GrpcChannel channel;
        public void connect()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7244");
            this.productCDSDTOClient = new ProductDTO.ProductDTOClient(channel);
            this.saleDTOClient = new SaleEnterpriseDTO.SaleEnterpriseDTOClient(channel);
        }

        public ProductDTOClient GetProductDTOClient()
        {
           return this.productCDSDTOClient;
        }

        public SaleEnterpriseDTO.SaleEnterpriseDTOClient GetSaleEnterpriseDTOClient()
        {
            return this.saleDTOClient;
        }
    }
}
