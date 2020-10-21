using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class GettingServerStatus : IClassFixture<CustomWebApplicationFactory>
    {

        private HttpClient _client;

        public GettingServerStatus(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        // do we get a 200 when we do a GET /serverstatus

        [Fact]
        public async void GetASuccessStatusCode()
        {
            var response = await _client.GetAsync("/serverstatus");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void IsAJsonResponse()
        {
            var response = await _client.GetAsync("/serverstatus");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async void HasProperResponse()
        {
            var response = await _client.GetAsync("/serverstatus");
            var content = await response.Content.ReadAsAsync<GetStatusResponse>();

            Assert.Equal("Looks Good", content.message);
            Assert.Equal("Joe Schmidt", content.checkedBy);
            var expectedDate = new DateTime(1969, 4, 20, 23, 59, 00);
            Assert.Equal(expectedDate, content.lastChecked);
        }

    }

    

    public class GetStatusResponse
    {
        public string message { get; set; }
        public string checkedBy { get; set; }
        public DateTime lastChecked { get; set; }
    }

}
