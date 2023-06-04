using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace StudentHelper.BL.Services.OtherServices
{
    public class GetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);

            return id;
        }
    }
}
