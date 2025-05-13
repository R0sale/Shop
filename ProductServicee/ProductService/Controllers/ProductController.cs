using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOObjects;

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
            var products = await _service.ProductService.GetAllProductsAsync(false);

            return Ok(products);
        }

        [HttpGet("{id:guid}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _service.ProductService.GetProductAsync(id, false);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDTO productForCreation)
        {
            var productDTO = await _service.ProductService.CreateProduct(productForCreation);

            return CreatedAtRoute("ProductById", new { id = productDTO.Id }, productDTO);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _service.ProductService.DeleteProduct(id, false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDTO productForUpd)
        {
            await _service.ProductService.UpdateProduct(id, productForUpd, true);

            return NoContent();
        }
    }
}
