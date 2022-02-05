using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public interface IStoreService
    {
        void init();
        void addStore(Store Store);
        Store getStore(int StoreId);
        List<Store> getStores();
    }
}
