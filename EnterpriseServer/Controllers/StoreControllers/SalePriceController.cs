using data.StoreData;
using Microsoft.AspNetCore.Mvc;
using services.StoreServices;

namespace EnterpriseServer.Controllers.StoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePriceController : ControllerBase
    {
        private readonly ILogger<SalePriceController> _logger;
        private IStoreService storeService;

        public SalePriceController(ILogger<SalePriceController> logger, IStoreService storeService)
        {
            _logger = logger;
            this.storeService = storeService;
        }

        [HttpPatch]
        [Route("/saleprice")]
        public void createProductSale()
        {
           
        }


    }
}
