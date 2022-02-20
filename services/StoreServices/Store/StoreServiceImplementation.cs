using data;
using data.EnterpriseData;
using data.StoreData;
using services.EnterpriseServices;
using Microsoft.EntityFrameworkCore;

namespace services.StoreServices
{
    public class StoreServiceImplementation : IStoreService
    {
        private IProductService productService;

        public StoreServiceImplementation(IProductService productService)
        {
            this.productService = productService;
        }

        public void init()
        {
            using (var context = new TradingsystemDbContext())
            {
                if (getStores(context).Count == 0)
                {
                    Product product1 = new Product();
                    product1.Name = "Sack Kartoffeln";
                    product1.Barcode = 9999;
                    product1.SellingPrice = 3.14;
                    product1.Id = 832467;

                    Store store1 = new Store();
                    store1.Name = "Edeka Nolte";
                    store1.Location = "Wiesbaden";
                    store1.Id = 1;

                    addStore(context, store1);
                    context.SaveChanges();
                }
            }

            using (var context = new TradingsystemDbContext())
            {
                if (getStockItemByStore(context, 1).Count == 0)
                {
                    StockItem stockItem1 = new StockItem();
                    stockItem1.SalesPrice = 0;
                    stockItem1.MinStock = 100;
                    stockItem1.MaxStock = 200;
                    stockItem1.Product = productService.getProductByBarcode(context, 1111);

                    addStockItemByStore(context, 1, stockItem1);
                    context.SaveChanges();
                }
            }

            using (var context = new TradingsystemDbContext())
            {
                if (getProductSales(context, 1).Count == 0)
                {
                    ProductSale productSale = new ProductSale();
                    productSale.Product = productService.getProductByBarcode(context, 1111);
                    productSale.SalePrice = 0.99;
                    //sproductSale.Store = getStore(context, 1);
                    addProductSales(context, 1, productSale);
                    context.SaveChanges();
                }
            }
        }

        public void addStore(TradingsystemDbContext context, Store Store)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                db.Add(Store);
                db.SaveChanges();
            }
        }

        public Store getStore(TradingsystemDbContext context, int StoreId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return db.Stores
                    .Include(s => s.ProductSales).ThenInclude(p => p.Product)
                    .Include(s => s.StockItems)
                    .Include(s => s.Sales)
                    .FirstOrDefault(p => p.Id == StoreId);
            }
        }

        public List<Store> getStores(TradingsystemDbContext context)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                if (db.Stores != null)
                {
                    return db.Stores.ToList();
                }
                else
                {
                    return new List<Store>();
                }
            }
        }

        public ProductSale getProductSaleByProductId(TradingsystemDbContext context, int StoreId, int ProductId)
        {
            foreach (var productSale in getProductSales(context, StoreId))
            {
                if (productSale.Product.Id == ProductId)
                {
                    return productSale;
                }
            }

            return null;
        }

        public List<ProductSale> getProductSales(TradingsystemDbContext context, int StoreId)
        {
            Store store = getStore(context, StoreId);
            return store.ProductSales.ToList();
        }

        public void addProductSales(TradingsystemDbContext context, int StoreId, ProductSale ProductSale)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Store store = getStore(db, StoreId);
                store.ProductSales.Add(ProductSale);
                //var productSaleEntity = db.ProductSales.Add(ProductSale);
                //var store = db.Stores.First(p => p.Id == StoreId);
                //store.ProductSales.Add(productSaleEntity.Entity);
                //db.Entry(store).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //db.Update(store);
                db.SaveChanges();
            }
        }

        public void updateProductSale(TradingsystemDbContext context, int StoreId, ProductSale ProductSale)
        {
            throw new NotImplementedException();
        }

        public void removeProductSale(TradingsystemDbContext context, int StoreId, int ProductSaleId)
        {
            throw new NotImplementedException();
        }

        public ProductSale getProductSaleById(TradingsystemDbContext context, int ProductSaleId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                ProductSale productSale =
                    db.ProductSales.Include(p => p.Product).FirstOrDefault(p => p.Id == ProductSaleId);
                return productSale;
            }
        }


        public List<StockItem> getStockItemByStore(TradingsystemDbContext context, int StoreId)
        {
            Store store = getStore(context, StoreId);
            return store.StockItems.ToList();
        }

        public void addStockItemByStore(TradingsystemDbContext context, int StoreId, StockItem StockItem)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Store store = getStore(db, StoreId);
                store.StockItems.Add(StockItem);
                db.SaveChanges();
            }
        }

        public StockItem? updateStockItemSalePrice(TradingsystemDbContext context, int storeId, int itemId, double salePrice)
        {
            using var db = TradingsystemDbContext.GetContext(context);
            Store store = getStore(db, storeId);
            var item = store.StockItems.Find(i => i.Id == itemId);
            if (item != null)
            {
                item.SalesPrice = salePrice;
                db.SaveChanges();
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}