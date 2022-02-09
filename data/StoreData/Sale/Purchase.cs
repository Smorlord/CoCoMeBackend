
namespace data.StoreData
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime SaleDateTime { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual List<PurchaseItem> PurchaseItems { get; set; }
    }
}
