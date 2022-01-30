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

        public Sale createSale(int storeId)
        {
            using (var db = new StoreDBContext())
            {
                Sale sale = new Sale();
                sale.saleDateTime = DateTime.Now;
                sale.store = storeService.getStore(storeId);
                db.SaveChanges();
                return sale;    
            }
        }

        public Sale updateSale(List<ProductSale> products)
        {
            throw new NotImplementedException();
        }
    }
}
