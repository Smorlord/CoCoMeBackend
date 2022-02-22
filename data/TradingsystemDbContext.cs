using data.EnterpriseData;
using data.StoreData;
using Microsoft.EntityFrameworkCore;
using static System.Environment;

namespace data
{
    public class TradingsystemDbContext : DbContext
    {
        public DbSet<OrderEntry> OrderEntries { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<DeliveryReports> DeliveryReports { get; set; } 
        public DbSet<ExchangeEntry> ExchangeEntry { get; set; }
        public DbSet<TradingEnterprise> TradingEnterprises { get; set; }
        public string DbPath { get; }

        public TradingsystemDbContext()
        {
            var folder = SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "tradingsystem.db");


            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite(@"DataSource=tradingsystem.db;");
            options.UseLazyLoadingProxies();
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected int UsingCount = 0;
        public static TradingsystemDbContext GetContext(TradingsystemDbContext context)
        {
            if (context != null)
            {
                context.UsingCount++;
            }
            else
            {
                context = new TradingsystemDbContext();
            }
            return context;
        }
        public override void Dispose()
        {
            if (UsingCount == 0)
            {
                base.Dispose();
            }
            else
            {
                UsingCount--;
            }
        }

        public override int SaveChanges()
        {
            if (UsingCount == 0)
            {
                return base.SaveChanges();
            }
            else
            {
                //return 0;
                return base.SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasMany(s => s.ProductSales)
                .WithOne(p => p.Store);
        }

    }
}
