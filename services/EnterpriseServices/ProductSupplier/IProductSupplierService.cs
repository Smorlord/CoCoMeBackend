using data;
using data.EnterpriseData;
namespace services.EnterpriseServices
{
    public interface IProductSupplierService
    {

        void addProductSupplier(TradingsystemDbContext context, ProductSupplier supplier, Product[] products);
        List<ProductSupplier> GetAllProducts(TradingsystemDbContext context);
    }
}
