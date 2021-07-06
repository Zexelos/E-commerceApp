using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using System.Net;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class EmployeePanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public EmployeePanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _clientAuth = _sut.GetEmployeeHttpClient();
            _clientUnauth = _sut.GetGuestHttpClient();
        }

        #region GET
        [Theory]
        [InlineData("EmployeePanel/Index")]
        [InlineData("EmployeePanel/Categories")]
        [InlineData("EmployeePanel/Products")]
        [InlineData("EmployeePanel/Orders")]
        [InlineData("EmployeePanel/OrderDetails")]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/UpdateCategory")]
        [InlineData("EmployeePanel/UpdateProduct")]
        [InlineData("EmployeePanel/DeleteCategory")]
        [InlineData("EmployeePanel/DeleteProduct")]
        [InlineData("EmployeePanel/DeleteOrder")]
        public async Task GetActions_EndpointsReturnRedirectToActionResultForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("EmployeePanel/Index")]
        [InlineData("EmployeePanel/Categories")]
        [InlineData("EmployeePanel/Products")]
        [InlineData("EmployeePanel/Orders")]
        [InlineData("EmployeePanel/Categories?selectedValue=Id&searchString=1&pageSize=10&pageNumber=1")]
        [InlineData("EmployeePanel/Products?selectedValue=Id&searchString=1&pageSize=10&pageNumber=1")]
        [InlineData("EmployeePanel/Orders?selectedValue=Id&searchString=1&pageSize=10&pageNumber=1")]
        [InlineData("EmployeePanel/OrderDetails/1")]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/UpdateCategory/1")]
        [InlineData("EmployeePanel/UpdateProduct/1")]
        public async Task GetActions_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("EmployeePanel/DeleteCategory/1")]
        [InlineData("EmployeePanel/DeleteProduct/1")]
        [InlineData("EmployeePanel/DeleteOrder/1")]
        public async Task GetActions_EndpointsReturnRedirectToActionForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("EmployeePanel/OrderDetails")]
        [InlineData("EmployeePanel/UpdateCategory")]
        [InlineData("EmployeePanel/UpdateProduct")]
        [InlineData("EmployeePanel/DeleteCategory")]
        [InlineData("EmployeePanel/DeleteProduct")]
        [InlineData("EmployeePanel/DeleteOrder")]
        public async Task GetActions_EndpointsReturnNotFoundWhenIdHasNoValueForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region POST
        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/UpdateCategory")]
        [InlineData("EmployeePanel/UpdateProduct")]
        public async Task PostActions_EndpointsReturnRedirectToActionResultForUnauthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // Act
            var response = await _clientUnauth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/UpdateCategory/1")]
        [InlineData("EmployeePanel/UpdateProduct/1")]
        public async Task PostActions_EndpointsReturnRedirectToActionResultForAuthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var categoryVM = new Dictionary<string, string>
            {
                { "Name", "pieczywo" },
                { "Description", "dobre" }
            };

            var productVM = new Dictionary<string, string>
            {
                { "Name", "chleb" },
                { "Description", "dobry" },
                { "UnitPrice", "10" },
                { "UnitsInStock", "2" },
            };

            if (url.Contains("Category"))
            {
                postRequest.Content = new FormUrlEncodedContent(categoryVM);
            }
            else
            {
                postRequest.Content = new FormUrlEncodedContent(productVM);
            }

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            if (url.Contains("Category"))
            {
                Assert.StartsWith("/EmployeePanel/Categories", response.Headers.Location.OriginalString);
            }
            else
            {
                Assert.StartsWith("/EmployeePanel/Products", response.Headers.Location.OriginalString);
            }
        }

        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/UpdateCategory/1")]
        [InlineData("EmployeePanel/UpdateProduct/1")]
        public async Task PostActions_EndpointsReturnBadRequestResultWhenModelStateIsNotValidForAuthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion
    }
}
