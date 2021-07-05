using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class CartControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public CartControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            // Arrange
            _sut = sut;
            _clientAuth = _sut.GetCustomerHttpClient();

            _clientUnauth = _sut.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("Cart/Index")]
        [InlineData("Cart/AddToCart?id=1&quantity=1")]
        [InlineData("Cart/IncreaseCartItemQuantityByOne?cartItemId=1")]
        [InlineData("Cart/DecreaseCartItemQuantityByOne?cartItemId=1")]
        [InlineData("Cart/DeleteCartItem?cartItemId=1")]
        public async Task AllActions_ReturnRedirectToActionResultForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("Cart/AddToCart/1?quantity=1")]
        [InlineData("Cart/IncreaseCartItemQuantityByOne?cartItemId=1")]
        [InlineData("Cart/DecreaseCartItemQuantityByOne?cartItemId=1")]
        [InlineData("Cart/DeleteCartItem?cartItemId=1")]
        public async Task Get_EndpointsReturnRedirectToActionResultForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Cart", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("Cart/IncreaseCartItemQuantityByOne")]
        [InlineData("Cart/DecreaseCartItemQuantityByOne")]
        [InlineData("Cart/DeleteCartItem")]
        public async Task Get_EndpointsReturnNotFoundRequestForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Index_ReturnSuccessAndCorrectContentTypeForAuthenticatedUser()
        {
            // Act
            var response = await _clientAuth.GetAsync("/Cart/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
