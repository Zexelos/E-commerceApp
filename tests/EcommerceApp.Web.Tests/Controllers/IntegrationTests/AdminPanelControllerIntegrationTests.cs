using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            // Arrange
            _sut = sut;
            _clientAuth = _sut.GetAdminHttpClient();
            _clientUnauth = _sut.GetGuestHttpClient();
        }

        [Theory]
        [InlineData("AdminPanel/Index")]
        [InlineData("AdminPanel/Employees")]
        [InlineData("AdminPanel/Customers")]
        [InlineData("AdminPanel/CustomerDetails")]
        [InlineData("AdminPanel/AddEmployee")]
        [InlineData("AdminPanel/UpdateEmployee")]
        [InlineData("AdminPanel/DeleteEmployee")]
        [InlineData("AdminPanel/DeleteCustomer")]
        public async Task AllActions_ReturnRedirectToActionResultForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("AdminPanel/Index")]
        [InlineData("AdminPanel/Employees")]
        [InlineData("AdminPanel/Employees?selectedValue=FirstName&searchString=ziutek&pageSize=10&pageNumber=1")]
        [InlineData("AdminPanel/Customers")]
        [InlineData("AdminPanel/Customers?selectedValue=FirstName&searchString=ziutek&pageSize=10&pageNumber=1")]
        [InlineData("AdminPanel/CustomerDetails/1")]
        [InlineData("AdminPanel/AddEmployee/1")]
        [InlineData("AdminPanel/UpdateEmployee/1")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("AdminPanel/AddEmployee")]
        [InlineData("AdminPanel/UpdateEmployee")]
        public async Task Post_EndpointsReturnBadRequest(string url)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var employeeVM = new Dictionary<string, string> { };
            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _clientAuth.SendAsync(postRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("AdminPanel/CustomerDetails")]
        [InlineData("AdminPanel/UpdateEmployee")]
        [InlineData("AdminPanel/DeleteEmployee")]
        [InlineData("AdminPanel/DeleteCustomer")]
        public async Task Get_EndpointsReturnNotFoundRequestForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/UpdateEmployee/1")]
        public async Task Post_EndpointsReturnRedirectWhenModelStateIsValidForAuthenticatedUser(string url)
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var employeeVM = new Dictionary<string, string>
            {
                {"Email" , "test@example.com"},
                {"Password" ,"Pa$$w0rd!"},
                {"FirstName" , "Integration"},
                {"LastName" , "Test"},
                {"Position", "Employee"}
            };
            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _clientAuth.SendAsync(postRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnRedirectToActionResultWhenIdHasValueForAuthenticatedUser()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await _clientAuth.GetAsync($"AdminPanel/DeleteEmployee/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnRedirectToActionResultWhenIdHasValueForAuthenticatedUser()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await _clientAuth.GetAsync($"AdminPanel/DeleteCustomer/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Customers", response.Headers.Location.OriginalString);
        }
    }
}