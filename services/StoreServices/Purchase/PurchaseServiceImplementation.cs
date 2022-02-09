using data;
using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public class PurchaseServiceImplementation : IPurchaseService
    {
        private IStoreService storeService;
        public PurchaseServiceImplementation(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public void init()
        {
        }

        public Purchase createPurchase(TradingsystemDbContext context, int storeId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Purchase purchase = new Purchase();
                purchase.SaleDateTime = DateTime.Now;
                purchase.StoreId = storeService.getStore(context, storeId).Id;
                db.Add(purchase);
                db.SaveChanges();
                return purchase;    
            }
        }

        public Purchase getPurchaseById(TradingsystemDbContext context, int purchaseId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Purchases.FirstOrDefault(s => s.Id == purchaseId);
            }
        }

        public Purchase updatePurchase(TradingsystemDbContext context, int purchaseId, List<PurchaseItem> purchaseItems)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Purchase purchase = getPurchaseById(db, purchaseId);
                purchase.PurchaseItems.AddRange(purchaseItems);
                db.SaveChanges();
                return purchase;
            }
        }
    }
}
