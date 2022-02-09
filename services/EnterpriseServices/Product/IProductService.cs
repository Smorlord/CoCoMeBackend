using data;
using data.EnterpriseData;

namespace services.EnterpriseServices
{
    public interface IProductService
    {

        void addProduct(TradingsystemDbContext context, Product Product);
        void removeProduct(TradingsystemDbContext context, int ProductId);
        Product getProduct(TradingsystemDbContext context, int ProductId);
        Product getProductByBarcode(TradingsystemDbContext context, int Barcode);
        List<Product> getProducts(TradingsystemDbContext context);

    }
}