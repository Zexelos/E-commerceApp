using System.Collections.Generic;
using System.Reflection.Metadata;
using System;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using System.Linq;
using EcommerceApp.Web.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("CanAccessEmployeePanel")]
    public class EmployeePanelController : Controller
    {
        private readonly ILogger<EmployeePanelController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public EmployeePanelController(
            ILogger<EmployeePanelController> logger,
            ICategoryService categoryService,
            IProductService productService,
            ISearchService searchService,
            IConfiguration configuration,
            IOrderService orderService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
            _searchService = searchService;
            _configuration = configuration;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

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
                return View(await _searchService.SearchPaginatedCategoriesAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return View(await _categoryService.GetPaginatedCategoriesAsync(intPageSize, pageNumber.Value));
        }

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
                return View(await _searchService.SearchPaginatedProductsAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return View(await _productService.GetPaginatedProductsAsync(intPageSize, pageNumber.Value));
        }

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
                return View(await _searchService.SearchPaginatedOrdersAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return View(await _orderService.GetPaginatedOrdersAsync(intPageSize, pageNumber.Value));
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            return View(await _orderService.GetOrderDetailsAsync(id.Value));
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _categoryService.AddCategoryAsync(categoryVM);
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _productService.AddProductAsync(productVM);
            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var model = await _categoryService.GetCategoryAsync(id.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var model = await _productService.GetProductAsync(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _categoryService.UpdateCategoryAsync(categoryVM);
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _productService.UpdateProductAsync(productVM);
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _categoryService.DeleteCategoryAsync(id.Value);
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _productService.DeleteProductAsync(id.Value);
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _orderService.DeleteOrderAsync(id.Value);
            return RedirectToAction(nameof(Orders));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
