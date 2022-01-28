using Microsoft.EntityFrameworkCore;
using StoreData.Data.ProductSale;
using static System.Environment;

namespace StoreData
{
    internal class StoreDBContext : DbContext
    {
        public DbSet<ProductSale> ProductSales { get; set; }

        public string DbPath { get; }

        public StoreDBContext()
        {
            var folder = SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "store.db");

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
