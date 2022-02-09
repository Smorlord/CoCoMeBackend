using GRPC_PurchaseStoreClient;

namespace cashDeskService.Printer
{
    public interface IPrinterService
    {
        void init();
        void printItems(List<ProductStoreDTOModel> ProductStoreDTOModels);
    }
}
