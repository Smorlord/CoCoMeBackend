namespace data.StoreData
{
    public class ProductOrder
    {

        public int Id { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public DateOnly OrderingDate { get; set; }

        public virtual List<OrderEntry> OrderEntries { get; set; }


    }
}