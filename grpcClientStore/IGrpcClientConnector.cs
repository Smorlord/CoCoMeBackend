using static GRPC_Client.ProductDTO;

namespace grpcClientStore
{
    public interface IGrpcClientConnector
    {
        void connect();
        ProductDTOClient GetProductDTOClient();
    }
}