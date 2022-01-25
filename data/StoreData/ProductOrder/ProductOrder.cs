namespace data.StoreData
{
    public class ProductOrder
    {

        public int Id { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public DateOnly OrderingDate { get; set; }

        public List<OrderEntry> OrderEntries = new List<OrderEntry>();


    }
}