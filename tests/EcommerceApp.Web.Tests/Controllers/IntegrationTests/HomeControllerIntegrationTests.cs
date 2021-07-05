using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class HomeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _client = _sut.GetGuestHttpClient();
        }

        [Theory]
        [InlineData("Home/Index")]
        [InlineData("Home/Privacy")]
        public async Task AllActions_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
