
using data.EnterpriseData;

namespace services.StoreService
{
    public interface ProductService
    {

        public void addProduct(Product Product);
        public void removeProduct(int ProductId);
        public Product getProduct(int ProductId);
        public Product getProductByBarcode(int Barcode);
        public List<Product> getProducts();

    }
}