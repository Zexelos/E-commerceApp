using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class HomeControllerUnitTests
    {
        private readonly HomeController _sut;
        private readonly Mock<ILogger<HomeController>> _logger = new();
        private readonly Mock<IHomeService> _homeService = new();

        public HomeControllerUnitTests()
        {
            _sut = new HomeController(
                _logger.Object,
                _homeService.Object
            );
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var homeVM = new HomeVM
            {
                Products = new List<ProductDetailsForHomeVM>
                {
                    new ProductDetailsForHomeVM
                    {
                        Name = "mleko"
                    }
                }
            };

            _homeService.Setup(s => s.GetHomeVMAsync()).ReturnsAsync(homeVM);

            // Act
            var result = await _sut.Index();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HomeVM>(viewResult.Model);
            Assert.Single(model.Products);
            Assert.Equal(homeVM.Products[0].Name, model.Products[0].Name);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _sut.Privacy();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
