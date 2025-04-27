using Microsoft.AspNetCore.Mvc;
using ProductManagement.Services.Interfaces;
using ProductManagement.Services.Models.Product;

namespace ProductManagement.WebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductSvc;
        public ProductController(IProductService _service) : base()
        {
            ProductSvc = _service;
        }

        [HttpGet("")]
        public async Task<ProductRS> GetProductsAsync([FromQuery] string category = null, [FromQuery] bool isActiveOnly = true)
        {
            var ret = await ProductSvc.GetProductsAsync(category, isActiveOnly);
            return ret;
        }

        [HttpGet("{id:int}")]
        public async Task<ProductRS> GetProductByIdAsync([FromRoute] int id)
        {
            var ret = await ProductSvc.GetProductByIdAsync(id);
            return ret;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProductsAsync([FromBody] ProductRQ request)
        {
            await ProductSvc.CreateProductAsync(request);
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProductByIdAsync([FromRoute] int id, [FromBody] ProductRQ request)
        {
            await ProductSvc.UpdateProductByIdAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromRoute] int id)
        {
            await ProductSvc.DeleteProductByIdAsync(id);
            return Ok();
        }
    }
}