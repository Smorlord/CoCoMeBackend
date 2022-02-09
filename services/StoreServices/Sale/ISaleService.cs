using data;
using data.StoreData;

namespace services.StoreServices
{
    public interface ISaleService
    {
        void init();
        Purchase createSale(TradingsystemDbContext context, int storeId);
        Purchase updateSale(TradingsystemDbContext context, int saleId, List<PurchaseItem> products);

        Purchase getSaleById(TradingsystemDbContext context, int saleId);
    }
}
