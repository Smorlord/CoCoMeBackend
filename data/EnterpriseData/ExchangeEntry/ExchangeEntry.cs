namespace data.EnterpriseData
{
    public class ExchangeEntry
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ExchangeAmount { get; set; }
        public virtual Product Product { get; set; }

    }
}
