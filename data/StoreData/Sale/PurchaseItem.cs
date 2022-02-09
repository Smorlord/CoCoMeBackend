using data.EnterpriseData;

namespace data.StoreData
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public double PurchasePrice { get; set; }
    }
}
