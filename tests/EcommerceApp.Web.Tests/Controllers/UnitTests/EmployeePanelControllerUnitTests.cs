using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Models;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests
{
    public class EmployeePanelControllerUnitTests
    {
        private readonly EmployeePanelController _sut;
        private readonly Mock<ILogger<EmployeePanelController>> _logger = new();
        private readonly Mock<ICategoryService> _categoryService = new();
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<ISearchService> _searchService = new();

        public EmployeePanelControllerUnitTests()
        {
            _sut = new EmployeePanelController(_logger.Object, _categoryService.Object, _productService.Object, _searchService.Object);
        }

        [Fact]
        public void Index_ReturnViewResult()
        {
            // Act
            var result = _sut.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Categories_ReturnViewResultWithAllCategories()
        {
            // Arrange
            var categoryVMs = GetCategoryVMs();

            _categoryService.Setup(s => s.GetCategoriesAsync()).ReturnsAsync(categoryVMs);

            // Act
            var result = await _sut.Categories(string.Empty, string.Empty);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<CategoryVM>>(viewResult.ViewData.Model);
            Assert.Equal(categoryVMs.Count, model.Count);
        }

        [Fact]
        public async Task Categories_ReturnViewResultWithSearchedCategories()
        {
            // Arrange
            var categoryVMs = GetCategoryVMs();

            _searchService.Setup(s => s.CategorySearchAsync("Name", "aciek")).ReturnsAsync(categoryVMs);

            // Act
            var result = await _sut.Categories("Name", "aciek");

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<CategoryVM>>(viewResult.ViewData.Model);
            Assert.Equal(categoryVMs[0].Name, model[0].Name);
            Assert.Equal(categoryVMs.Count, model.Count);
        }

        [Fact]
        public async Task Products_ReturnViewResultWithAllProducts()
        {
            // Arrange
            var productVMs = GetProductVMs();

            _productService.Setup(s => s.GetProductsAsync()).ReturnsAsync(productVMs);

            // Act
            var result = await _sut.Products(string.Empty, string.Empty);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProductVM>>(viewResult.ViewData.Model);
            Assert.Equal(productVMs.Count, model.Count);
        }

        [Fact]
        public async Task Products_ReturnViewResultWithSearchedProducts()
        {
            // Arrange
            var productVMs = GetProductVMs();

            _searchService.Setup(s => s.ProductSearchAsync("Name", "aciek")).ReturnsAsync(productVMs);

            // Act
            var result = await _sut.Products("Name", "aciek");

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProductVM>>(viewResult.ViewData.Model);
            Assert.Equal(productVMs[0].Name, model[0].Name);
            Assert.Equal(productVMs.Count, model.Count);
        }

        [Fact]
        public void AddCategory_GET_ReturnViewModel()
        {
            // Act
            var result = _sut.AddCategory();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddProduct_GET_ReturnViewResult()
        {
            // Act
            var result = _sut.AddProduct();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddCategory_POST_ReturnBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var categoryVM = new CategoryVM();
            _sut.ModelState.AddModelError("error", "idk");

            // Act
            var result = await _sut.AddCategory(categoryVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddCategory_POST_ReturnRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            var categoryVM = new CategoryVM { Id = 100, Name = "argrarg", Description = "sadwegwe" };

            // Act
            var result = await _sut.AddCategory(categoryVM);

            // Assert
            _categoryService.Verify(v => v.AddCategoryAsync(categoryVM), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Categories), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task AddProduct_POST_ReturnBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var productVM1 = new ProductVM();

            _sut.ModelState.AddModelError("error", "idk");
            // Act
            var result = await _sut.AddProduct(productVM1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddProduct_POST_ReturnRedirectToActionResultWhenModelStateIsValid()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "argrarg", Description = "sadwegwe" };

            // Act
            var result = await _sut.AddProduct(productVM);

            // Assert
            _productService.Verify(v => v.AddProductAsync(productVM), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Products), redirectToActionResult.ActionName);
        }

        // Helper methods
        private static List<CategoryVM> GetCategoryVMs()
        {
            var categoryVM1 = new CategoryVM { Id = 100, Name = "Maciek", Description = "sadwegwe" };
            var categoryVM2 = new CategoryVM { Id = 150, Name = "aciek", Description = "xwafx" };
            var categoryVM3 = new CategoryVM { Id = 200, Name = "asdfaciekasge", Description = "2xt42xy" };

            return new List<CategoryVM> { categoryVM1, categoryVM2, categoryVM3 };
        }

        private static List<ProductVM> GetProductVMs()
        {
            var productVM1 = new ProductVM { Id = 100, Name = "Maciek", Description = "sadwegwe" };
            var productVM2 = new ProductVM { Id = 150, Name = "aciek", Description = "xwafx" };
            var productVM3 = new ProductVM { Id = 200, Name = "asdfaciekasge", Description = "2xt42xy" };

            return new List<ProductVM> { productVM1, productVM2, productVM3 };
        }
    }
}
