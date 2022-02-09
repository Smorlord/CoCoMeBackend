using data;
using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public class SaleServiceImplementation : ISaleService
    {
        private IStoreService storeService;
        public SaleServiceImplementation(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public void init()
        {
        }

        public Purchase createSale(TradingsystemDbContext context, int storeId)
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

        public Purchase getSaleById(TradingsystemDbContext context, int saleId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Purchases.FirstOrDefault(s => s.Id == saleId);
            }
        }

        public Purchase updateSale(TradingsystemDbContext context, int saleId, List<PurchaseItem> purchaseItems)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Purchase purchase = getSaleById(db, saleId);
                purchase.PurchaseItems.AddRange(purchaseItems);
                db.SaveChanges();
                return purchase;
            }
        }
    }
}
