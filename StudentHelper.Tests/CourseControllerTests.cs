using Azure.Core;
using StudentHelper.Tests.Extensions;
using System.Net;

namespace StudentHelper.Tests
{
    public class CourseControllerTests
    {
        private readonly HttpClient _client;
        private readonly GetToken _tokenService;
        public CourseControllerTests(GetToken tokenService)
        {
            var application = new MyWebApplication();
            _tokenService = tokenService;
            _client = application.CreateClient();
        }
        [Fact]
        public async Task GetAllCoursesReturnsOk()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/products");

            request.Headers.Add("Authorization", $"Bearer {_tokenService.GetAccessTokenAsync()}");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}