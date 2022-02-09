using data;
using data.StoreData;

namespace services.StoreServices
{
    public interface IPurchaseService
    {
        void init();
        Purchase createPurchase(TradingsystemDbContext context, int storeId);
        Purchase updatePurchase(TradingsystemDbContext context, int purchaseId, List<PurchaseItem> products);

        Purchase getPurchaseById(TradingsystemDbContext context, int purchaseId);
    }
}
