using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class CartControllerUnitTests
    {
        private readonly CartController _sut;
        private readonly Mock<ILogger<CartController>> _logger = new();
        private readonly Mock<ICartItemService> _cartItemService = new();

        public CartControllerUnitTests()
        {
            _sut = new CartController(
                _logger.Object,
                _cartItemService.Object
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var cartItemListVM = new CartItemListVM
            {
                TotalPrice = 22.32m,
                CartItems = new List<CartItemForListVM>
                {
                    new CartItemForListVM
                    {
                        Name = "frytki"
                    }
                }
            };

            _cartItemService.Setup(s => s.GetCartItemListAsync(It.IsAny<string>())).ReturnsAsync(cartItemListVM);

            // Act
            var result = await _sut.Index();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartItemListVM>(viewResult.Model);
            Assert.Single(model.CartItems);
            Assert.Equal(cartItemListVM.TotalPrice, model.TotalPrice);
        }

        [Fact]
        public async Task AddToCart_ReturnsRedirectToActionResult()
        {
            // Act
            var result = await _sut.AddToCart(1, 1);

            // Assert
            _cartItemService.Verify(v => v.AddCartItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task IncreaseCartItemQuantityByOne_ReturnsRedirectToActionResult()
        {
            // Arrange
            var cartItemId = 10;

            // Act
            var result = await _sut.IncreaseCartItemQuantityByOne(cartItemId);

            // Assert
            _cartItemService.Verify(v => v.IncreaseCartItemQuantityByOneAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DecreaseCartItemQuantityByOne_ReturnsRedirectToActionResult()
        {
            // Arrange
            var cartItemId = 10;

            // Act
            var result = await _sut.DecreaseCartItemQuantityByOne(cartItemId);

            // Assert
            _cartItemService.Verify(v => v.DecreaseCartItemQuantityByOneAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteCartItem_ReturnsRedirectToActionResult()
        {
            // Arrange
            var cartItemId = 10;

            // Act
            var result = await _sut.DeleteCartItem(cartItemId);

            // Assert
            _cartItemService.Verify(v => v.DeleteCartItemAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
