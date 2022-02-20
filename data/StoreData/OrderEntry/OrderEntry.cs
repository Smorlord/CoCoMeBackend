namespace data.StoreData
{
    public class OrderEntry
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual ProductOrder ProductOrder { get; set; }
        public int Amount { get; set; }
    }

    public class OrderEntryRequest
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}