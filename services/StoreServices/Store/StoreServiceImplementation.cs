using data;
using data.StoreData;

namespace services.StoreServices
{
    public class StoreServiceImplementation : IStoreService
    {

        public StoreServiceImplementation()
        {
            
        }

        public void init()
        {
            if (getStores().Count == 0)
            {
                Store store1 = new Store();
                store1.Name = "Edeka Nolte";
                store1.Location = "Wiesbaden";
                store1.Id = 1;

                addStore(store1);

            }
        }

        public void addStore(Store Store)
        {
            using (var db = new TradingsystemDbContext())
            {
                db.Add(Store);
                db.SaveChanges();
            }
        }

        public Store getStore(int StoreId)
        {
            using (var db = new TradingsystemDbContext())
            {
                return db.Stores.First(p => p.Id == StoreId);
            }

        }

        public List<Store> getStores()
        {
            using (var db = new TradingsystemDbContext())
            {
                if (db.Stores != null)
                {
                    return db.Stores.ToList();
                }
                else
                {
                    return new List<Store>();
                }
                
            }

        }
    }
}
