using GRPC_Client;

namespace cashDeskService.Session
{
    public interface ISessionService
    {
        void init();
        void updatePurchaseId(int purchaseId);
        int getStoreId();
        int getPurchaseId();

        void addScannedProduct(ProductScannedDTOModel scannedProduct);

        void clearScannedProduct();
        List<ProductScannedDTOModel> getScannedProducts();

        void setTotalPrice(double totalPrice);
        double getTotalPrice();

        Boolean getExpressCheckOut();
        void setExpressCheckOut(Boolean expressCheckOut);
    }
}
