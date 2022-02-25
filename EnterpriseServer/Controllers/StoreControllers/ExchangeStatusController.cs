using data;
using data.EnterpriseData;
using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using services.StoreServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeStatusController
    {

        private IStoreService storeService;

        public ExchangeStatusController(ILogger<OrderController> logger, IStoreService storeService) {
            this.storeService = storeService;
        }

        [HttpPost]
        [Route("exchange-status")]
        public void changeExchangeStatus(int storeId)
        {
            storeService.changeExchangeStatusInStockItem(storeId);

        }
    }
}
