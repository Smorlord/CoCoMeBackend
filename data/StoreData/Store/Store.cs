using data.EnterpriseData;

namespace data.StoreData
{
    public class Store
    {

        public int Id { get; set;  }
        public string Name { get; set; }
        public int Location { get; set; } // PLZ-Gebiete 0-9

        public virtual List<ProductSale> ProductSales { get; set; }
        public virtual List<StockItem> StockItems { get; set; }
        public virtual List<Purchase> Sales { get; set; }

        public bool ProductIsInDelivery { get; set; }
        public virtual List<ExchangeEntry> ExchangeEntry { get; set; }

    }
}