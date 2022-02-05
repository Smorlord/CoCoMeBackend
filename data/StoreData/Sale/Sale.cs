
namespace data.StoreData
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDateTime { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
        //public List<ProductSale> ProductSales { get; } = new List<ProductSale>();
    }
}
