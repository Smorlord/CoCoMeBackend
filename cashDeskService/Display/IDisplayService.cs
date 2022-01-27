using GRPC_Client;

namespace cashDeskService.Display
{
    public interface IDisplayService
    {
        void init();
        void showInDisplay(ProductScannedDTOModel item);
    }
}
