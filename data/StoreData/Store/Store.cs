namespace data.StoreData
{
    public class Store
    {

        public int Id { get; set;  }
        public string Name { get; set; }
        public string Location { get; set; }

        public virtual List<ProductSale> ProductSales { get; set; }
        public virtual List<StockItem> StockItems { get; set; }
        public virtual List<Purchase> Sales { get; set; }

    }
}