using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Requests;
using StudentHelper.WebApi.Service;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AdminRole> _roleManager;
        private readonly EmailService _emailService;
        private readonly SMTPConfig _config;
        private readonly IWebHostEnvironment _environment;


        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<AdminRole> roleManager, EmailService emailService, SMTPConfig config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
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

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<Response> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.RecipientEmail);
            if (user == null)
            {
                return new Response(400, false, "User does not exist.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);
            var emailRequest = new EmailRequest
            {
                Body = "Reset Password",
                Subject = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>",
                RecipientEmail = request.RecipientEmail
            };
            // You can use any email service provider to send email here
            await _emailService.SendEmailAsync(emailRequest, _config);

            return new Response(200, true, $"{callbackUrl}");
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<Response> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Response(400, false, "User does not exist.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
            {
                return new Response(200, true, "Password has been reset successfully.");
            }
            return new Response(400, false, "Failed to reset password.");
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