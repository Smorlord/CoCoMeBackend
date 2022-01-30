using data.StoreData;

namespace services.StoreServices
{
    public interface ISaleService
    {
        Sale createSale(int storeId);
        Sale updateSale(List<ProductSale> products);
    }
}
