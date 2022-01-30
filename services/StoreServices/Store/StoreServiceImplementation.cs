using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public class StoreServiceImplementation : IStoreService
    {

        public StoreServiceImplementation()
        {
            if (getStores().Count == 0)
            {
                Store store1 = new Store();
                store1.Name = "Edeka Nolte";
                store1.Location = "Wiesbaden";

                addStore(store1);
               
            }
        }

        public void addStore(Store Store)
        {
            using (var db = new StoreDBContext())
            {
                db.Add(Store);
                db.SaveChanges();
            }
        }

        public Store getStore(int StoreId)
        {
            using (var db = new StoreDBContext())
            {
                return db.Stores.First(p => p.Id == StoreId);
            }

        }

        public List<Store> getStores()
        {
            using (var db = new StoreDBContext())
            {
                return db.Stores.ToList();
            }

        }
    }
}
