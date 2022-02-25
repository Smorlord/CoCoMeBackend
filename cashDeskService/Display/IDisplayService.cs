using GRPC_Client;

namespace cashDeskService.Display
{
    public interface IDisplayService
    {
        void init();
        void showItemInDisplay(ProductScannedDTOModel item);

        void showTotalInDisplay(double totalAmount);

        void showStartPurchase(int saleId);
        void showFinishPurchase();

        void showNoPurchase();

        void changeExpressCheckOut(Boolean expressCheckOut);
        void showIsExpressLock();

        void showNoFinish();

        void showNoPay();

        void purchaseAlreadyExist();
    }
}
