using data;
using data.EnterpriseData;
using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using services.StoreServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private IStoreService storeService;

        public StoreController(ILogger<StoreController> logger, IStoreService storeService)
        {
            _logger = logger;
            this.storeService = storeService;
        }

        [HttpGet]
        [Route("/stock")]
        public List<StockItem> GetStockItems(int storeId)
        {
            List<StockItem> items = storeService.getStockItemByStore(null, storeId);
            return items;
        }

        [HttpGet]
        [Route("/sales")]
        public List<ProductSale> GetSales(int storeId)
        {
            List<ProductSale> items = storeService.getProductSales(null, storeId);
            return items;
        }
    }
}