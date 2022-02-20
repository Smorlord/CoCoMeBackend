using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public interface IOrderService
    {
        void addProductOrder(ProductOrder productOrder);
        ProductOrder getProductOrder(int productOrderId);
        void removeProductOrder(int productOrderId);
        List<ProductOrder> getAllProductOrders();
    }
}
