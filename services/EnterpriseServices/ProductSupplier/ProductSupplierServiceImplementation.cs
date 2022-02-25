using data;
using data.EnterpriseData;
using Microsoft.EntityFrameworkCore;

namespace services.EnterpriseServices
{
    public class ProductSupplierServiceImplementation : IProductSupplierService
    {
        public void addProductSupplier(TradingsystemDbContext context, ProductSupplier supplier, Product[] products)
        {
            using var db = TradingsystemDbContext.GetContext(context);
            var createdSupplier = db.Add(supplier);
            createdSupplier.Entity.Products = new List<Product>(products);
            db.SaveChanges();
        }

        public List<ProductSupplier> GetAllProducts(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.ProductSuppliers != null ? db.ProductSuppliers.Include(s => s.Products).ToList() : new List<ProductSupplier>();
            }
        }
    }
}
