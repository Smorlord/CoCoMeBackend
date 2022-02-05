namespace StoreServices.StoreService
{
    public interface OrderService
    { 
        public int createOrder();
        public void getOrderById(int oderId);

        public void updateIventoryByOder(int orderId);
    }
}