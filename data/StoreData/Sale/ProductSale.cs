using data.EnterpriseData;

namespace data.StoreData
{
    public class ProductSale
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public double SalePrice { get; set; }
    }
}
