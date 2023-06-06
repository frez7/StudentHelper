using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.UserRequests;
using StudentHelper.WebApi.Data;
using StudentHelper.WebApi.Extensions;
using StudentHelper.WebApi.Managers;
using StudentHelper.WebApi.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthManager _authManager;

        public AuthController(AuthManager authManager) { 
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<AuthResponse> Authenticate([FromBody] LoginRequest request)
        {
            return await _authManager.Authenticate(request);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            return await _authManager.Register(request);
        }

        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        public async Task<TokenModel> RefreshToken(TokenModel? tokenModel)
        {
            return await _authManager.RefreshToken(tokenModel);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<Response> ForgotPassword(ForgotPasswordRequest request)
        {
            return await _authManager.ForgotPassword(request);
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public async Task<Response> ResetPassword(string userId, string token)
        {
            return await _authManager.ResetPassword(userId, token);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<UserResponse> GetCurrentUser()
        {
            return await _authManager.GetCurrentUser();
        }

        [HttpGet("users/{userId}")]
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _authManager.GetUserById(userId);
        }

        [HttpPost("ChangeCurrentPassword")]
        public async Task<Response> ChangeCurrentPassword(ChangePasswordRequest request)
        {
            return await _authManager.ChangeCurrentPassword(request);
        }
    }
}