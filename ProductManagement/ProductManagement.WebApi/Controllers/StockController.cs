using Microsoft.AspNetCore.Mvc;
using ProductManagement.Services.Interfaces;

namespace ProductManagement.WebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService StockSvc;
        public StockController(IStockService _service) : base() 
        {
            StockSvc = _service;
        }

        [HttpPut("add-to-stock/{id:int}/{quantity:int}")]
        public async Task<IActionResult> IncrementProductStockByIdAsync([FromRoute] int id, [FromRoute] int quantity)
        {
            await StockSvc.UpdateProductStockByIdAsync(id, quantity, true);
            return Ok();
        }

        [HttpPut("decrement-stock/{id:int}/{quantity:int}")]
        public async Task<IActionResult> DecrementProductStockByIdAsync([FromRoute] int id, [FromRoute] int quantity)
        {
            await StockSvc.UpdateProductStockByIdAsync(id, quantity, false);
            return Ok();
        }
    }
}