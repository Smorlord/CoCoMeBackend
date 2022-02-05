using data.EnterpriseData;
using data.StoreData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace data
{
    public class TradingsystemDbContext : DbContext
    {
        public DbSet<OrderEntry> OrderEntries { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }

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
            => options.UseSqlite($"Data Source={DbPath}");

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .(b => b.Url)
                .IsRequired();
        }*/
    }
}
