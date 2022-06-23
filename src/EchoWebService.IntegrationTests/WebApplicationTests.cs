using System;
using System.Net.Http;
using System.Threading.Tasks;
using EchoWebService.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace EchoWebService.IntegrationTests
{
    /// <summary>
    /// Integration tests for the web application.
    /// </summary>
    public sealed class WebApplicationTests : Foundatio.Logging.Xunit.TestWithLoggingBase
    {
        private readonly WebApplicationFactory<Startup> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplicationTests"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        /// <param name="factory">Factory method for the web application.</param>
        public WebApplicationTests(
            ITestOutputHelper output,
            WebApplicationFactory<Startup> factory)
            : base(output)
        {
            _factory = factory;
        }

        /// <summary>
        /// Checks that GET requests work.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HttpGetReturnsSuccessAndCorrectContentTypeAsync()
        {
            var client = _factory.CreateClient();
            var requestUri = new Uri("/");
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "text/text; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            await LogResponseAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Helper method to log the http response.
        /// </summary>
        /// <param name="response">Http Response.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task LogResponseAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
                _logger.LogInformation("No HTTP Response Message.");
                return;
            }

            foreach (var (key, value) in response.Headers)
            {
                _logger.LogInformation($"{key}: {string.Join(",", value)}");
            }

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation(result);
        }
    }
}
