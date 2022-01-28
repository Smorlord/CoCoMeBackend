using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using StoreServices.StoreService;

namespace StoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIService : ControllerBase
    {
        private readonly ILogger<OrderAPIService> _logger;
        private OrderService orderService;


        public OrderAPIService(ILogger<OrderAPIService> logger, OrderService orderService)
        {
            _logger = logger;
            this.orderService = orderService;
        }

        [HttpPost]
        [Route("/order")]
        public int CreateOrder(List<OrderEntry> orderEntries)
        {
           return orderService.createOrder(orderEntries);
        }

        [HttpGet]
        [Route("/order/{id}")]
        public ProductOrder GetOrderById(int id)
        {
            return orderService.getOrderById(id);
        }

        [HttpPost]
        [Route("/order/update/{id}")]
        public void UpdateIventoryByOder(int id)
        {
            orderService.updateIventoryByOder(id);
        }


    }
}