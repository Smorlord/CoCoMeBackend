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
            return storeService.getStockItemByStore(null, storeId);
        }
    }
}