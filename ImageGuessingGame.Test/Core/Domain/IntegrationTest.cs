using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ImageGuessingGame.Test
{
    public class BasicTests 
        : IClassFixture<WebApplicationFactory<ImageGuessingGame.Startup>>
    {
        private readonly WebApplicationFactory<ImageGuessingGame.Startup> _factory;

        public BasicTests(WebApplicationFactory<ImageGuessingGame.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/LoginUsers/Login")]
        [InlineData("/LoginUsers/Register")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }
    }
}