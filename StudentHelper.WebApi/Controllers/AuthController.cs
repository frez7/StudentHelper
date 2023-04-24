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
        private readonly RoleManager<Role> _roleManager;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<Response> Login(LoginRequest request)
        {
            var result = await _signInManager
                .PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, lockoutOnFailure: false);
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
            var result = await RegisterUser(request);
            
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(request.UserName, request.Password, isPersistent: true, lockoutOnFailure: false);
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

        [Authorize(Roles ="Admin, User")]
        [HttpGet("GetCurrentUser")]
        public async Task<UserResponse> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            
            if (user == null)
            {
                return new UserResponse(404, false, "Not Found!", "", "", "", null);
            }
            return new UserResponse(200, true, $"User info:", user.UserName, user.Email, user.Id, roles.ToList());
        }

        [HttpPost("ChangeCurrentPassword")]
        public async Task<Response> ChangeCurrentPassword(ChangePasswordRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return new Response(400, false, "Неудачная попытка смены пароля, перепроверьте введенные данные!");
            }
            return new Response(200, true, "Вы успешно сменили свой пароль, запомните его!");
        }
        private void SetUserProperties(User user, string email, string username)
        {
            user.UserName = username;
            user.Email = email;
        }
        private async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {

            var user = new User();
            var role = new Role { Name = "Admin" };
            await _roleManager.CreateAsync(role);
            
            SetUserProperties(user, request.Email, request.UserName);
           
            var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "Admin");

            return result;
        }
    }
}
