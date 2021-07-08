using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePanelController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public EmployeePanelController(
            ICategoryService categoryService,
            IProductService productService,
            ISearchService searchService,
            IConfiguration configuration,
            IOrderService orderService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _searchService = searchService;
            _configuration = configuration;
            _orderService = orderService;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> Categories(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchPaginatedCategoriesAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _categoryService.GetPaginatedCategoriesAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Products")]
        public async Task<IActionResult> Products(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchPaginatedProductsAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _productService.GetPaginatedProductsAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> Orders(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchPaginatedOrdersAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _orderService.GetPaginatedOrdersAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("OrderDetails/{id}")]
        public async Task<IActionResult> OrderDetails([FromRoute] int id)
        {
            return Ok(await _orderService.GetOrderDetailsAsync(id));
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryVM categoryVM)
        {
            await _categoryService.AddCategoryAsync(categoryVM);
            return Ok();
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductVM productVM)
        {
            await _productService.AddProductAsync(productVM);
            return Ok();
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryVM categoryVM)
        {
            await _categoryService.UpdateCategoryAsync(categoryVM);
            return Ok();
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductVM productVM)
        {
            await _productService.UpdateProductAsync(productVM);
            return Ok();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }
    }
}
