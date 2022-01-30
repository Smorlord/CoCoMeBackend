using Microsoft.EntityFrameworkCore;
using static System.Environment;

namespace data.StoreData
{
    public class StoreDBContext : DbContext
    {

        public DbSet<OrderEntry> OrderEntries { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        public string DbPath { get; }

        public StoreDBContext()
        {
            var folder = SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "enterprise.db");

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

