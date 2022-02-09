using GRPC_Client;

namespace cashDeskService.Display
{
    public interface IDisplayService
    {
        void init();
        void showItemInDisplay(ProductScannedDTOModel item);

        void showTotalInDisplay(double totalAmount);

        void showStartSale(int saleId);
        void showFinishSale();
    }
}
