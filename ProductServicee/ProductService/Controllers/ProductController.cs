using Azure;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOObjects;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.JsonPatch;
using ProductService.ActionFilters;
using Shared.Request;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParams productParams)
        {
            var products = await _service.ProductService.GetAllProductsAsync(productParams, false);

            return Ok(products);
        }

        [HttpGet("{id:guid}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _service.ProductService.GetProductAsync(id, false);

            return Ok(product);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDTO productForCreation)
        {
            var productDTO = await _service.ProductService.CreateProduct(productForCreation);

            return CreatedAtRoute("ProductById", new { id = productDTO.Id }, productDTO);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _service.ProductService.DeleteProduct(id, User, false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDTO productForUpd)
        {
            await _service.ProductService.UpdateProduct(id, productForUpd, User, true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<ProductForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("The patchDoc is null.");

            var result = await _service.ProductService.GetProductForPatialUpdate(id, User, true);
            patchDoc.ApplyTo(result.productForUpd);

            TryValidateModel(result.productForUpd);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.ProductService.SaveChangesForPatrialUpdate(result.productForUpd, result.productEntity);
            return NoContent();
        }
    }
}
