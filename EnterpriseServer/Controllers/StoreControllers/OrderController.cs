using data;
using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using services.EnterpriseServices;
using services.StoreServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController
    {
        private readonly ILogger<OrderController> _logger;
        private IStoreService storeService;
        private IOrderService orderService;

        public OrderController(
            ILogger<OrderController> logger,
            IStoreService storeService,
            IOrderService orderService)
        {
            _logger = logger;
            this.storeService = storeService;
            this.orderService = orderService;
        }

        /* UC 3 - order product - response: ProductOrder */
        [HttpPost]
        [Route("/order/create")]
        public ProductOrder CreateProductOrder(TradingsystemDbContext context, int storeId, int productId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                OrderEntry orderEntry = new OrderEntry();
                orderEntry.Id = productId;

                Store store = storeService.getStore(context, storeId);
                foreach (StockItem item in store.StockItems)
                {
                    if (item.Id == productId)
                    {
                        orderEntry.Amount = item.MaxStock - item.Amount; // Automatically orders the amount of products to fill up the maxStock 
                    }
                }

                ProductOrder productOrder = new ProductOrder();
                productOrder.DeliveryDate = DateTime.Now.AddDays(12);
                productOrder.OrderingDate = DateTime.Now;
                productOrder.OrderEntries.Add(orderEntry);
                productOrder.StoreId = storeId;

                orderService.addProductOrder(productOrder);

                return productOrder;
            }
        }

        /* UC 4 - receive ordered products - response: ProductOrder */
        [HttpPut]
        [Route("/order/updatestock")]
        public void UpdateStockItems(TradingsystemDbContext context, int storeId, int productOrderId)
        {
            using (var db = TradingsystemDbContext.GetContext(context))
            {
                // get Productorder by id
                ProductOrder productOrder = orderService.getProductOrder(productOrderId);

                // get store by id and update amount
                Store store = storeService.getStore(context,storeId);
                foreach (StockItem item in store.StockItems)
                {
                    if (productOrder.StoreId == storeId)
                    {
                        // The ordered quantities are always up to the maxStock and
                        item.Amount = item.MaxStock; // are automatically replenished accordingly. The StoreManager
                                                     // must manually check the delivery for completeness.
                    }
                }

                // delete ProductOrderById
                orderService.removeProductOrder(productOrderId);
            }
        }


    }
}
