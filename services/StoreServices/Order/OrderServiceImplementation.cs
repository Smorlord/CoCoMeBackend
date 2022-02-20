using data;
using data.StoreData;
using Microsoft.EntityFrameworkCore;

namespace services.StoreServices
{
    public class OrderServiceImplementation : IOrderService
    {
        public void addProductOrder(ProductOrder productOrder)
        {
            using (var db = new TradingsystemDbContext())
            {
                db.Add(productOrder);
                db.SaveChanges();
            }
        }

        public ProductOrder getProductOrder(int productOrderId)
        {
            using (var db = new TradingsystemDbContext())
            {
                return db.ProductOrders.Include(p => p.OrderEntries).First(p => p.Id == productOrderId);
            }
        }

        public void removeProductOrder(int productOrderId)
        {
            using (var db = new TradingsystemDbContext())
            {
                db.Remove(getProductOrder(productOrderId));
                db.SaveChanges();
            }
        }
    }
}
