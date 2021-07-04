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
    public class ProductControllerUnitTests
    {
        private readonly ProductController _sut;
        private readonly Mock<ILogger<ProductController>> _logger = new();
        private readonly Mock<IProductService> _productService = new();

        public ProductControllerUnitTests()
        {
            _sut = new ProductController(
                _logger.Object,
                _productService.Object
            );
        }

        [Fact]
        public async Task Product_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.Product(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Product_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var productDetailsForUserVM = new ProductDetailsForUserVM
            {
                Name = "mleko"
            };

            _productService.Setup(s =>
                s.GetProductDetailsForUserAsync(It.IsAny<int>())).ReturnsAsync(productDetailsForUserVM);

            // Act
            var result = await _sut.Product(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDetailsForUserVM>(viewResult.Model);
            Assert.Equal(productDetailsForUserVM.Name, model.Name);
        }
    }
}
