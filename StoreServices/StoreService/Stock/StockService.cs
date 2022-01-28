using data.StoreData;

namespace StoreServices.StoreService
{
    public interface StockService
    {
        public List<StockItem> getStockItemsByStoreId(int storeId);
    }
}
