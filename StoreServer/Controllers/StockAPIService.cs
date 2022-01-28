using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using StoreServices.StoreService;

namespace StoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAPIService : ControllerBase
    {
        private readonly ILogger<StockAPIService> _logger;
        private StockService stockService;


        public StockAPIService(ILogger<StockAPIService> logger, StockService stockService)
        {
            this._logger = logger;
            this.stockService = stockService;
        }

        [HttpGet]
        [Route("/stock/{id}")]
        public List<StockItem> GetStockItemsByStoreId(int id)
        {
            return stockService.getStockItemsByStoreId(id);
        }
    }
}
