using Microsoft.AspNetCore.Mvc;
using StoreData.Data.ProductSale;
using StoreServices.StoreService;

namespace StoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIService : ControllerBase
    {
        private readonly ILogger<ProductAPIService> _logger;
        private ProductSaleService productSaleService;

        public ProductAPIService(ILogger<ProductAPIService> logger, ProductSaleService productSaleService)
        {
            _logger = logger;
            this.productSaleService = productSaleService;
        }

        [HttpGet]
        [Route("/product")]
        public void CreateProductSale(ProductSale productSale)
        {
            productSaleService.createProductSale(productSale);
        }

    }
}