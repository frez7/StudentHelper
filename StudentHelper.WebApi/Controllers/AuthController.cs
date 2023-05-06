using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Requests;
using StudentHelper.WebApi.Data;
using StudentHelper.WebApi.Extensions;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly EmailService _emailService;
        private readonly SMTPConfig _config;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, EmailService emailService, SMTPConfig config, ITokenService tokenService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return BadRequest(new Response(401, false, "Данный пользователь не существует!"));
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest(new Response(401, false, "Неверный пароль!"));
            }

            var identityRoles = new List<IdentityRole<int>>();
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                identityRoles.Add(role);
            }
            var accessToken = _tokenService.CreateToken(user, identityRoles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());
            await _userManager.UpdateAsync(user);
            return Ok(new AuthResponse(200, true, "Операция успешна!"
                , accessToken, user.RefreshToken, user.UserName));
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (!result.Succeeded) 
                return BadRequest(new Response(400, false, "Произошла некая ошибка при создании нового пользователя!"));


            var role = new IdentityRole<int> { Name = "User" };
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, "User");

            return await Authenticate(new LoginRequest
            {
                UserName = request.UserName,
                Password = request.Password
            });
        }
        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel? tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest(new Response(400, false, "Ошибка на стороне клиента!"));
            }

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;
            var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return BadRequest(new Response(400, false, "Неверный \"Access\" или \"Refresh\" токен!"));
            }

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new Response(400, false, "Неверный \"Access\" или \"Refresh\" токен!"));
            }

            var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenModel { AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken), RefreshToken = newRefreshToken, });
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
            await _emailService.SendEmailAsync(emailRequest, _config);

            return new Response(200, true, $"{callbackUrl}");
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public async Task<Response> ResetPassword(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response(400, false, "User does not exist.");
            }
            var newPass = GeneratePassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);
            var emailRequest = new EmailRequest
            {
                Body = "Сброс пароля",
                Subject =
                $"UserName: {user.UserName}<br/>" +
                $"Password: {newPass}<br/>" +
                $"Email: {user.Email}",
                RecipientEmail = user.Email,
            };
            if (result.Succeeded)
            {
                await _emailService.SendEmailAsync(emailRequest, _config);
                return new Response(200, true, "Вы успешно сбросили свой пароль, он был отправлен на вашу почту!");
            }
            return new Response(400, false, "Failed to reset password.");
        }

        [Authorize(Roles ="Admin, User, Manager")]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserResponse>> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(404, false, "Некорректный запрос!"));
            }
            return new UserResponse(200, true, $"Вы успешно вывели текущего пользователя!", user.UserName, user.Email, user.Id, roles.ToList());
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
        private string GeneratePassword()
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            const int passwordLength = 8;

            var randNum = new Random();
            var chars = new char[passwordLength];
            var allowedCharCount = allowedChars.Length;

            chars[0] = allowedChars[randNum.Next(0, 25)];
            chars[1] = allowedChars[randNum.Next(26, 51)];
            chars[2] = allowedChars[randNum.Next(52, 61)];

            for (int i = 3; i < passwordLength; i++)
            {
                chars[i] = allowedChars[randNum.Next(0, allowedCharCount)];
            }

            return new string(chars);
        }
    }
}