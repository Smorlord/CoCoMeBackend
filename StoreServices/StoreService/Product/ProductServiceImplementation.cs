using data.EnterpriseData;
using services.EnterpriseServices;

namespace services.EnterpriseService
{
    public class ProductServiceImplementation : ProductService
    {

        public ProductServiceImplementation()
        {
            if (getProducts().Count == 0)
            {
                Product cookie = new Product();
                cookie.Name = "Keks";
                cookie.Barcode = 1111;
                cookie.PurchasePrice = 1.99;

                Product chocolate = new Product();
                chocolate.Name = "Schokolade";
                chocolate.Barcode = 2222;
                chocolate.PurchasePrice = 2.49;

                Product chips = new Product();
                chips.Name = "Chips";
                chips.Barcode = 3333;
                chips.PurchasePrice = 3.19;


                addProduct(cookie);
                addProduct(chocolate);
                addProduct(chips);
            }
        }

        public void addProduct(Product Product)
        {
            using (var db = new EnterpriseDBContext())
            {
                db.Add(Product);
                db.SaveChanges();
            }
        }

        public void removeProduct(int ProductID)
        {
            using (var db = new EnterpriseDBContext())
            {
                db.Remove(getProduct(ProductID));
                db.SaveChanges();
            }
            
        }

        public Product getProduct(int ProductId)
        {
            using (var db = new EnterpriseDBContext())
            {
                return db.Products.First(p => p.Id == ProductId);
            }
            
        }

        public List<Product> getProducts()
        {
            using (var db = new EnterpriseDBContext())
            {
                return db.Products.ToList();
            }
            
        }

        public Product getProductByBarcode(int Barcode)
        {
            using (var db = new EnterpriseDBContext())
            {
                return db.Products.First(p => p.Barcode == Barcode);
            }
        }
    }
}