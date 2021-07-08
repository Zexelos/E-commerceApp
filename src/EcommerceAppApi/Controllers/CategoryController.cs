using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductService _productService;

        public CategoryController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Products/{categoryId}")]
        public async Task<IActionResult> Products([FromRoute] int categoryId)
        {
            return Ok(await _productService.GetProductsByCategoryIdAsync(categoryId));
        }
    }
}
