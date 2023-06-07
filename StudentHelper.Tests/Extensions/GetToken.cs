using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace StudentHelper.Tests.Extensions
{
    public class GetToken
    {
        private readonly HttpClient _client;

        public GetToken()
        {
            var application = new MyWebApplication();
            _client = application.CreateClient();
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "http://frez773-001-site1.atempurl.com/api/Auth/Login",
                ClientId = "27",
                ClientSecret = "string",
            });
            return tokenResponse.AccessToken;
        }
    }
}
