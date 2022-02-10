namespace data.StoreData
{
    public class ProductOrder
    {

        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderingDate { get; set; }

        public virtual List<OrderEntry> OrderEntries { get; set; }


    }
}