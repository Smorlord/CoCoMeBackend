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

        public ExchangeStatusController(ILogger<OrderController> logger, IStoreService storeService,) {
            this.storeService = storeService;
        }

        [HttpPost]
        [Route("exchange-status")]
        public void changeExchangeStatus(int storeId)
        {
            using (var db = new TradingsystemDbContext())
            {
                Store reciever = storeService.getStore(db, storeId);

                reciever.StockItems.ForEach(recieverItem =>
                {
                    reciever.ExchangeEntry.ForEach( exchangeEntry =>
                    {
                        if(recieverItem.Product.Id == exchangeEntry.Product.Id) 
                        {
                            recieverItem.Amount += exchangeEntry.ExchangeAmount;
                            recieverItem.ExchangeStatus = null;

                            storeService.getStore(db, exchangeEntry.StoreId).StockItems.ForEach( supplierItem => 
                            {
                                if(supplierItem.Product.Id == exchangeEntry.Product.Id)
                                {
                                    supplierItem.Amount -= exchangeEntry.ExchangeAmount;
                                    supplierItem.ExchangeStatus = null;
                                }
                            });
                        }
                        
                    });
                });

            }

        }
    }
}
