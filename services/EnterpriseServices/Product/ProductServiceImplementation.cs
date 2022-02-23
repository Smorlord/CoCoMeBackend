using data;
using data.EnterpriseData;

namespace services.EnterpriseServices
{
    public class ProductServiceImplementation : IProductService
    {
        public ProductServiceImplementation()
        {
            using (var context = new TradingsystemDbContext())
            {
                if (getProducts(context).Count == 0)
                {
                    Product cookie = new Product();
                    cookie.Id = 1;
                    cookie.Name = "Keks";
                    cookie.Barcode = 1111;
                    cookie.SellingPrice = 1.99;

                    Product chocolate = new Product();
                    chocolate.Id = 2;
                    chocolate.Name = "Schokolade";
                    chocolate.Barcode = 2222;
                    chocolate.SellingPrice = 2.49;

                    Product chips = new Product();
                    chips.Id = 3;
                    chips.Name = "Chips";
                    chips.Barcode = 3333;
                    chips.SellingPrice = 3.19;


                    addProduct(context, cookie);
                    addProduct(context, chocolate);
                    addProduct(context, chips);

                    addProductSupplier(context, new ProductSupplier { Name = "Supplier 1" }, new Product[]{
                        cookie, chocolate,
                    });
                    addProductSupplier(context, new ProductSupplier { Name = "Supplier 2" }, new Product[]{chips});
                    context.SaveChanges();
                }
            }
        }

        private void addProductSupplier(TradingsystemDbContext context, ProductSupplier supplier,
            Product[] products)
        {
            using var db = TradingsystemDbContext.GetContext(context);
            var createdSupplier = db.Add(supplier);
            createdSupplier.Entity.Products = new List<Product>(products);
            db.SaveChanges();
        }

        public void addProduct(TradingsystemDbContext context, Product Product)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Add(Product);
                db.SaveChanges();
            }
        }

        public void removeProduct(TradingsystemDbContext context, int ProductID)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Remove(getProduct(db, ProductID));
                db.SaveChanges();
            }
        }

        public Product getProduct(TradingsystemDbContext context, int ProductId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Products.FirstOrDefault(p => p.Id == ProductId);
            }
        }

        public List<Product> getProducts(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Products != null ? db.Products.ToList() : new List<Product>();
            }
        }

        public Product getProductByBarcode(TradingsystemDbContext context, int Barcode)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Products.FirstOrDefault(p => p.Barcode == Barcode);
            }
        }
    }
}