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

        public ProductOrder getProductOrder(int productOrderId, TradingsystemDbContext? context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.ProductOrders.Include(p => p.OrderEntries).First(p => p.Id == productOrderId);
            }
        }

        public void removeProductOrder(int productOrderId)
        {
            using (var db = new TradingsystemDbContext())
            {
                db.Remove(getProductOrder(productOrderId, db));
                db.SaveChanges();
            }
        }

        public List<ProductOrder> getAllProductOrders()
        {
            using (var db = new TradingsystemDbContext())
            {
                return db.ProductOrders != null ? db.ProductOrders.ToList() : new List<ProductOrder>();
            }
        }

        public List<ProductOrder> getAllProductOrdersByStoreId(int storeId)
        {
            using (var db = new TradingsystemDbContext())
            {
                return db.ProductOrders != null ? db.ProductOrders.Where(p => p.StoreId == storeId).Include(p => p.OrderEntries).ToList() : new List<ProductOrder>();
            }
        }

        public void setDeliveryDateToday(int productOrderId)
        {
            using var db = new TradingsystemDbContext();
            var order = getProductOrder(productOrderId, db);
            order.DeliveryDate = DateTime.Now;
            db.SaveChanges();
        }
    }
}
