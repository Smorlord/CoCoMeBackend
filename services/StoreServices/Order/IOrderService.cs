using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;

namespace services.StoreServices
{
    public interface IOrderService
    {
        void addProductOrder(ProductOrder productOrder);
        ProductOrder getProductOrder(int productOrderId, TradingsystemDbContext? context);
        void removeProductOrder(int productOrderId);
        List<ProductOrder> getAllProductOrders();
        List<ProductOrder> getAllProductOrdersByStoreId(int storeId);

        void setDeliveryDateToday(int productOrderId);
    }
}
