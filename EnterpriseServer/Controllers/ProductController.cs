using data.EnterpriseData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services.EnterpriseServices;

namespace EnterpriseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        private IProductService productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            this.productService = productService;
        }

        /* UC 3 - order product - response: all products */
        [HttpGet]
        [Route("/products")]
        public List<Product> GetAllProducts()
        {
            return productService.getProducts(null);
        }

        [HttpGet]
        [Route("/product/{id}")]
        public Product GetProductById(int id)
        {
            return productService.getProduct(null, id);
        }

        [HttpPost]
        [Route("/product")]
        public void AddProduct(Product product)
        {
            productService.addProduct(null, product);
        }

        [HttpDelete]
        [Route("/product/{id}")]
        public void DeleteProduct(int id)
        {
            productService.removeProduct(null, id);
        }
    }
}
