using data.StoreData;

namespace services.StoreServices
{
    public interface ISaleService
    {
        void init();
        Sale createSale(int storeId);
        Sale updateSale(int saleId, List<ProductSale> products);

        Sale getSaleById(int saleId);
    }
}
