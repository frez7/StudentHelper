using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Models.Entities;
using StudentHelper.WebApi.Extensions;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace StudentHelper.WebApi.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(User user, List<IdentityRole<int>> roles)
        {
            var token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
