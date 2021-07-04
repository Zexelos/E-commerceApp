using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class OrderControllerUnitTests
    {
        private readonly OrderController _sut;
        private readonly Mock<IOrderService> _orderService = new();
        private readonly Mock<IConfiguration> _configuration = new();

        public OrderControllerUnitTests()
        {
            _sut = new OrderController(
                _orderService.Object,
                _configuration.Object
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
        public async Task Checkout_GET_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.Checkout(customerId: null);

            // Assert
            Assert.NotNull(result);
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task Checkout_GET_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var orderCheckoutVM = new OrderCheckoutVM
            {
                Address = "makowa 2",
                CartItems = new List<CartItemForOrderCheckoutVM>
                {
                    new CartItemForOrderCheckoutVM
                    {
                        Name = "mleko"
                    }
                }
            };

            _orderService.Setup(s => s.GetOrderCheckoutVMAsync(It.IsAny<int>())).ReturnsAsync(orderCheckoutVM);

            // Act
            var result = await _sut.Checkout(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderCheckoutVM>(viewResult.Model);
            Assert.Single(orderCheckoutVM.CartItems);
            Assert.Equal(orderCheckoutVM.Address, model.Address);
            Assert.Equal(orderCheckoutVM.CartItems[0].Name, model.CartItems[0].Name);
        }

        [Fact]
        public async Task Checkout_POST_ReturnsBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var orderCheckoutVM = new OrderCheckoutVM();

            _sut.ModelState.AddModelError("error", "idk");

            // Act
            var result = await _sut.Checkout(orderCheckoutVM);

            // Assert
            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Checkout_POST_ReturnsRedirectToActionResultWhenModelStateIsValid()
        {
            // Arrange
            var orderCheckoutVM = new OrderCheckoutVM()
            {
                CartId = 1
            };

            // Act
            var result = await _sut.Checkout(orderCheckoutVM);

            // Assert
            _orderService.Verify(v => v.AddOrderAsync(It.IsAny<OrderCheckoutVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.CheckoutSuccessful), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task CustomerOrderDetails_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.CustomerOrderDetails(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CustomerOrderDetails_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var customerOrderDetailsVM = new CustomerOrderDetailsVM
            {
                OrderItems = new List<OrderItemForCustomerOrderDetailVM>
                {
                    new OrderItemForCustomerOrderDetailVM
                    {
                        Quantity = 2
                    }
                },
                ContactEmail = "234@fddfs.com"
            };

            _orderService.Setup(s =>
                s.GetCustomerOrderDetailsAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(customerOrderDetailsVM);

            // Act
            var result = await _sut.CustomerOrderDetails(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerOrderDetailsVM>(viewResult.Model);
            Assert.Single(model.OrderItems);
            Assert.Equal(customerOrderDetailsVM.ContactEmail, model.ContactEmail);
            Assert.Equal(customerOrderDetailsVM.OrderItems[0].Quantity, model.OrderItems[0].Quantity);
        }

        [Fact]
        public void CheckoutSuccessful_ReturnsViewResult()
        {
            // Act
            var result = _sut.CheckoutSuccessful();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
