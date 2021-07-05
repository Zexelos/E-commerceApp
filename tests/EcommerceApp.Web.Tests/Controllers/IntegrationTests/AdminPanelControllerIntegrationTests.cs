using System.Net;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Infrastructure;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clinetAuth;
        private readonly HttpClient _clientUnauth;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            // Arrange
            _sut = sut;
            _clinetAuth = _sut.GetAdminPanelHttpClient();

            _clientUnauth = _sut.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
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

        [Fact]
        public async Task Index_ReturnViewResultForAuthenticatedUser()
        {
            // Act
            var response = await _clinetAuth.GetAsync("AdminPanel/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Employees Management</a>", content);
            Assert.Contains("Customers Management</a>", content);
        }

        [Fact]
        public async Task Employees_ReturnsViewResultForAuthenticatedUser()
        {

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/Employees");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<th>First Name</th>", content);
            Assert.Contains("<th>Position</th>", content);
        }

        [Fact]
        public async Task Customers_ReturnsViewResultForAuthenticatedUser()
        {
            // Act
            var response = await _clinetAuth.GetAsync("AdminPanel/Customers");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<th>First Name</th>", content);
            Assert.Contains("<th>PhoneNumber</th>", content);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsViewResultForAuthenticatedUser()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/CustomerDetails/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<label for=\"LastName\">Last name</label>", content);
            Assert.Contains("<label for=\"PostalCode\">Postal code</label>", content);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsNotFoundObjectResultWhenIdHasNoValueForAuthenticatedUser()
        {
            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/CustomerDetails");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddEmployee_GET_ReturnViewResultForAuthenticatedUser()
        {
            // Act
            var response = await _clinetAuth.GetAsync("AdminPanel/AddEmployee");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<p>Fill in the fields and create a new employee</p>", content);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnRedirectToActionResultWhenModelStateIsValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "AdminPanel/AddEmployee");

            var employeeVM = new Dictionary<string, string>
            {
                {"Email", "test@example.com"},
                {"Password", "Pa$$w0rd!"},
                {"FirstName", "Zordon"},
                {"LastNAme", "Rasista"},
                {"Position", "wf43t234"}
            };

            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            // Act
            var response = await _clinetAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnBadRequestResultWhenModelStateIsNotValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "AdminPanel/AddEmployee");

            var employeeVM = new Dictionary<string, string>
            {
                {"Email", "a"},
                {"Password", "a"},
                {"FirstName", "a"},
                {"LastNAme", "a"},
                {"Position", "a"}
            };

            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            // Act
            var response = await _clinetAuth.SendAsync(postRequest);

            // Assert
            Assert.NotEqual(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnViewResultWhenIdHasValueForAuthenticatedUser()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/UpdateEmployee/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<p>Fill in the fields to update current employee</p>", content);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnNotFoundResultWhenIdHasNoValueForAuthenticatedUser()
        {
            // Arrange
            int? id = null;

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/UpdateEmployee/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_POST_ReturnRedirectResultWhenModelStateIsValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "AdminPanel/UpdateEmployee");

            var employeeVM = new Dictionary<string, string>
            {
                {"Id", "1"},
                {"Email", "test@example.com"},
                {"Password", "Pa$$w0rd!"},
                {"FirstName", "Zordon"},
                {"LastNAme", "Rasista"},
                {"Position", "wf43t234"}
            };

            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            // Act
            var response = await _clinetAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task UpdateEmployee_POST_ReturnBadRequestResultWhenModelStateIsNotvalidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "AdminPanel/UpdateEmployee");

            var employeeVM = new Dictionary<string, string>
            {
                {"Id", "1"},
                {"Email", "0"},
                {"Password", "0"},
                {"FirstName", "0"},
                {"LastNAme", "0"},
                {"Position", "0"}
            };

            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            // Act
            var response = await _clinetAuth.SendAsync(postRequest);

            // Assert
            Assert.NotEqual(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnRedirectToActionResultWhenIdHasValueForAuthenticatedUser()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/DeleteEmployee/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnNotFoundResultWhenIdHasNoValueForAuthenticatedUser()
        {
            // Arrange
            int? id = null;

            // Act
            var response = await _clinetAuth.GetAsync($"AdminPanel/UpdateEmployee/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}