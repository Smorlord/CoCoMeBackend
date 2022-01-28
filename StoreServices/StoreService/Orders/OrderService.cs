using data.StoreData;

namespace StoreServices.StoreService
{
    public interface OrderService
    { 
        public int createOrder(List<OrderEntry> orderEntries);
        public ProductOrder getOrderById(int oderId);

        public void updateIventoryByOder(int orderId);
    }
}