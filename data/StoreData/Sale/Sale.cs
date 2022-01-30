
namespace data.StoreData
{
    public class Sale
    {
        public int id { get; set; }
        public DateTime saleDateTime { get; set; }
        public Store store { get; set; }
        public List<ProductSale> products { get; } = new List<ProductSale>();
    }
}
