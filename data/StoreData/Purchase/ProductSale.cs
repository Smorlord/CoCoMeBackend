using data.EnterpriseData;

namespace data.StoreData
{
    public class ProductSale
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public double SalePrice { get; set; }
    }
}
