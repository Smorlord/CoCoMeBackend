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

        public Sale createSale(int storeId)
        {
            using (var db = new TradingsystemDbContext())
            {
                Sale sale = new Sale();
                sale.SaleDateTime = DateTime.Now;
                sale.StoreId = storeService.getStore(storeId).Id;
                db.Add(sale);
                db.SaveChanges();
                return sale;    
            }
        }

        public Sale getSaleById(int saleId)
        {
            using (var db = new TradingsystemDbContext())
            {
                return db.Sales.First(s => s.Id == saleId);
            }
        }

        public Sale updateSale(int saleId, List<ProductSale> products)
        {
            using (var db = new TradingsystemDbContext())
            {
                Sale sale = getSaleById(saleId);
                //sale.ProductSales.AddRange(products);
                db.SaveChanges();
                return sale;
            }
        }
    }
}
