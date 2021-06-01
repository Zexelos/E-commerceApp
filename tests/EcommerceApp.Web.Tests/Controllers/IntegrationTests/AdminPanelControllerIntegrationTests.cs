using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            //Arrange
            _sut = sut;
            _client = _sut.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task Index_ReturnsListOfEmployeesVM()
        {
            //Act
            var response = await _client.GetAsync("AdminPanel/Index");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<th>Id</th>", content);
            /*
            Assert.Contains("<th>Email</th>",content);
            Assert.Contains("<th>First Name</th>",content);
            Assert.Contains("<th>Last Name</th>",content);
            Assert.Contains("<th>Position</th>",content); */
        }
    }
}