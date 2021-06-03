using System.Reflection.Metadata;
using System;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("Admin, Employee")]
    public class EmployeePanelController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public EmployeePanelController(ILogger logger, ICategoryService categoryService, IProductService productService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var model = await _categoryService.GetCategoriesAsync();
            return View(model);
        }

        public async Task<IActionResult> Products()
        {
            var model = await _productService.GetProductsAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var model = await _categoryService.GetCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _categoryService.AddCategoryAsync(categoryVM);
            return RedirectToAction(nameof(AddCategory));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _productService.AddProductAsync(productVM);
            return RedirectToAction(nameof(AddProduct));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Category ID in the route");
            }
            var model = await _categoryService.GetCategoryAsync(id.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route");
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
                return NotFound("You must pass a valid Category ID in the route");
            }
            await _categoryService.DeleteCategoryAsync(id.Value);
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route");
            }
            await _productService.DeleteProductAsync(id.Value);
            return RedirectToAction(nameof(Products));
        }
    }
}
