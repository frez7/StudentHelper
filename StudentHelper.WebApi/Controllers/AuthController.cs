using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Requests;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<Response> Login(LoginRequest request)
        {
            var result = await _signInManager
                .PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return new Response(200, true, "User voshel v sistemu");
            }
            return new Response(400, false, "Invalid login attempt.");
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<Response> Register(RegisterRequest request)
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await RegisterUser(request);
            
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: true, lockoutOnFailure: false);
                return new Response(200, true, "User has been registered");
            }
            return new Response(400, false, $"You ne zaregalsya something error");
        }

        [HttpPost("Logout")]
        public async Task<Response> Logout()
        {
            await _signInManager.SignOutAsync();
            return new Response(200, true, "Logout completed");
        }

        [HttpGet("GetCurrentUser")]
        public async Task<Response> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new Response(404, false, "Not Found!");
            }
            return new Response(200, true, $"ID: {user.Id}, EMAIL: {user.Email}, USERNAME: {user.UserName}");
        }
        private void SetUserProperties(User user, string email)
        {
            user.Email = email;
            user.UserName = email;
        }
        private async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {
            var user = new User();
            SetUserProperties(user, request.Email);

            var result = await _userManager.CreateAsync(user, request.Password);

            return result;
        }
    }
}
