using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class OrderControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public OrderControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _clientAuth = _sut.GetCustomerHttpClient();
            _clientUnauth = _sut.GetGuestHttpClient();
        }

        [Theory]
        [InlineData("Order/Checkout")]
        [InlineData("Order/CustomerOrders")]
        [InlineData("Order/CustomerOrderDetails")]
        [InlineData("Order/CheckoutSuccessful")]
        public async Task GetActions_EndpointsReturnRedirectResultForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("Order/Checkout?customerId=1")]
        [InlineData("Order/CustomerOrders?pageSize=10&pageNumber=1")]
        [InlineData("Order/CustomerOrderDetails/1")]
        [InlineData("Order/CheckoutSuccessful")]
        public async Task GetActions_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task CustomerOrderDetails_EndpointsReturnNotFoundResultWhenIdHasNoValueForAuthenticatedUser()
        {
            // Act
            var response = await _clientAuth.GetAsync("Order/CustomerOrderDetails");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CheckoutPost_EndpointsReturnRedirectToActionResultWhenModelStateIsValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Order/Checkout");
            var orderCheckoutVM = new Dictionary<string, string>
            {
                { "CartId", "1" },
                { "CustomerId", "1" },
                { "TotalPrice", "10" },
                { "FirstName", "maciek" },
                { "LastName", "mackowski" },
                { "City", "maciekowo" },
                { "PostalCode", "11111" },
                { "Address", "mackowa 1/1" },
                { "Email", "maciek@maciek.pl" },
                { "PhoneNumber", "123123123" },
                { "CartItems[0].ProductId", "1" },
                { "CartItems[0].Quantity", "1" },
            };
            postRequest.Content = new FormUrlEncodedContent(orderCheckoutVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("/Order/CheckoutSuccessful", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task CheckoutPost_EndpointsReturnBadRequestResultWhenModelStateIsNotValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Order/Checkout");
            var orderCheckoutVM = new Dictionary<string, string>
            {
            };
            postRequest.Content = new FormUrlEncodedContent(orderCheckoutVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
