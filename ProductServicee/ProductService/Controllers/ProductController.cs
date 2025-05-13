using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProductController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet(Name = "Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            throw new Exception("CHlen");

            var products = await _service.ProductService.GetAllProductsAsync(false);

            return Ok(products);
        }

        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _service.ProductService.GetProductAsync(id, false);

            return Ok(product);
        }
    }
}
