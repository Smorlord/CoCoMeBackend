using data;
using data.StoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.StoreServices
{
    public interface IStoreService
    {
        void init();
        void addStore(TradingsystemDbContext context, Store Store);
        Store getStore(TradingsystemDbContext context, int StoreId);
        List<Store> getStores(TradingsystemDbContext context);

        ProductSale getProductSaleByProductId(TradingsystemDbContext context, int StoreId, int ProductId);
        List<ProductSale> getProductSales(TradingsystemDbContext context, int StoreId);
        void updateProductSale(TradingsystemDbContext context, int StoreId, int ProductId, double SalesPrice);
        void removeProductSale(TradingsystemDbContext context, int StoreId, int ProductSaleId);

        ProductSale getProductSaleById(TradingsystemDbContext context, int ProductSaleId);

        List<StockItem> getStockItemByStore(TradingsystemDbContext context, int StoreId);
        StockItem? getStockItemByStoreByProduct(TradingsystemDbContext context, int StoreId, int ProductId);
        void updateStockItemsInStore(TradingsystemDbContext context, int storeId, List<OrderEntry> entries);
        void addStockItemByStore(TradingsystemDbContext context, int StoreId, StockItem StockItem);
    }
}