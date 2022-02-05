namespace data.StoreData
{
    public class Store
    {

        public int Id { get; set;  }
        public string Name { get; set; }
        public string Location { get; set; }

        public List<StockItem> StockItems = new List<StockItem>();
        public List<Sale> Sales = new List<Sale>();

    }
}