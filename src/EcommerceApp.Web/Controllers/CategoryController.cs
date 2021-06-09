using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IProductService _productService;

        public CategoryController(ILogger<CategoryController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Products(string categoryName)
        {
            var model = await _productService.GetProductsByCategoryNameAsync(categoryName);
            return View(model);
        }
    }
}
