using GRPC_Client;

namespace cashDeskService.Session
{
    public interface ISessionService
    {
        void init();
        void updateSaleId(int saleId);
        int getStoreId();
        int getSaleId();

        void addScannedProduct(ProductScannedDTOModel scannedProduct);

        void clearScannedProduct();
        List<ProductScannedDTOModel> getScannedProducts();

        void setTotalPrice(double totalPrice);
        double getTotalPrice();
    }
}
