using data.EnterpriseData;

namespace services.EnterpriseServices
{
    public interface ProductService
    {

        void addProduct(Product Product);
        void removeProduct(int ProductId);
        Product getProduct(int ProductId);
        Product getProductByBarcode(int Barcode);
        List<Product> getProducts();

    }
}