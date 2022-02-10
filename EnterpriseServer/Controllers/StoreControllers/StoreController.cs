using data;
using data.EnterpriseData;
using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using services.StoreServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController
    {
        private readonly ILogger<StoreController> _logger;
        private IStoreService storeService;

        public StoreController(ILogger<StoreController> logger, IStoreService storeService)
        {
            _logger = logger;
            this.storeService = storeService;
        }

        /* UC 3 - order product - response: outOfStock Products*/
        [HttpGet]
        [Route("/outofstock")]
        public List<StockItem> GetOutOfStockItems(TradingsystemDbContext context, int storeId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                Store store = storeService.getStore(context,storeId);
                List<StockItem> outOfStock = new List<StockItem>();

                foreach (StockItem item in store.StockItems)
                {
                    if (item.Amount < item.MinStock)
                    {
                        outOfStock.Add(item);
                    }
                }
                return outOfStock;
            }
        }

        /* UC 5 - stock report - response: Store */
        [HttpGet]
        [Route("/stockreport")]
        public Store GetStockItems(TradingsystemDbContext context, int storeId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                return storeService.getStore(context,storeId);  // In a real production, you would send the
                                                        // necessary data to reduce the risk
            }
        }

        
        


    }
}
