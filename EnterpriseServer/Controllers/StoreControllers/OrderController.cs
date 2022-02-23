using data.StoreData;
using Microsoft.AspNetCore.Mvc;
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
        public ProductOrder CreateProductOrder(int storeId, List<OrderEntryRequest> entries)
        {
            List<OrderEntry> orderEntries = entries.ConvertAll(e => new OrderEntry
                {
                    ProductId = e.ProductId,
                    Amount = e.Amount
                }
            );
            ProductOrder productOrder = new ProductOrder();
            productOrder.OrderingDate = DateTime.Now;
            productOrder.OrderEntries = orderEntries;
            productOrder.StoreId = storeId;

            orderService.addProductOrder(productOrder);

            return productOrder;
        }

        /* UC 4 - receive ordered products - response: ProductOrder */
        [HttpPost]
        [Route("/order/updatestock")]
        public void UpdateStockItems(int storeId, int productOrderId)
        {
            ProductOrder productOrder = orderService.getProductOrder(productOrderId);

            storeService.updateStockItemsInStore(null, storeId, productOrder.OrderEntries);

            orderService.removeProductOrder(productOrderId);
        }
    }
}