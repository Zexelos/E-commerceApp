using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class CategoryControllerUnitTests
    {
        private readonly CategoryController _sut;
        private readonly Mock<ILogger<CategoryController>> _logger = new();
        private readonly Mock<IProductService> _productService = new();

        public CategoryControllerUnitTests()
        {
            _sut = new CategoryController(
                _logger.Object,
                _productService.Object
            );
        }

        [Fact]
        public async Task Products_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.Products(categoryId: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Products_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var listProductDetailsForUserVM = new ListProductDetailsForUserVM
            {
                Products = new List<ProductDetailsForUserVM>
                {
                    new ProductDetailsForUserVM
                    {
                        Name = "mleko",
                        UnitPrice = 11m
                    }
                }
            };

            _productService.Setup(s => s.GetProductsByCategoryIdAsync(It.IsAny<int>())).ReturnsAsync(listProductDetailsForUserVM);

            // Act
            var result = await _sut.Products(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListProductDetailsForUserVM>(viewResult.Model);
            Assert.Single(model.Products);
            Assert.Equal(listProductDetailsForUserVM.Products[0].UnitPrice, model.Products[0].UnitPrice);
            Assert.Equal(listProductDetailsForUserVM.Products[0].Name, model.Products[0].Name);
        }
    }
}
