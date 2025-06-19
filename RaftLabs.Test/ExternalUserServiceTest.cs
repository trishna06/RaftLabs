using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RaftLabs.Application.Configuration;
using RaftLabs.Application.Services;
using RaftLabs.Domain.Models;
using System.Net;
using System.Text.Json.Serialization;

namespace Raftlabs.Test
{
    public class ExternalUserServiceTest
    {
        [Test]
        public async Task GetUserByIdAsync_ReturnsUser_WhenSuccessful()
        {
            // Arrange
            MockHttpMessageHandler handler = new MockHttpMessageHandler(@"
            {
              ""data"": {
                ""id"": 1,
                ""email"": ""test@reqres.in"",
                ""first_name"": ""John"",
                ""last_name"": ""Doe""
              }
            }", HttpStatusCode.OK);

            HttpClient httpClient = new HttpClient(handler);

            IOptions<ApiSettings> options = Options.Create(new ApiSettings { BaseUrl = "https://reqres.in/api", CacheExpirationSeconds = 60 });
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

            ExternalUserService service = new ExternalUserService(httpClient, options, memoryCache);

            // Act
            User user = await service.GetUserByIdAsync(1);

            // Assert
            user.Should().NotBeNull();
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.Email.Should().Be("test@reqres.in");
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var message = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response)
            };
            return Task.FromResult(message);
        }
    }
}
