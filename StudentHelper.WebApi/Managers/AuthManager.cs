using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using StudentHelper.BL.Logging;
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.Other;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests;
using StudentHelper.WebApi.Data;
using StudentHelper.WebApi.Extensions;
using StudentHelper.WebApi.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;

namespace StudentHelper.WebApi.Managers
{
    public class AuthManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly EmailService _emailService;
        private readonly SMTPConfig _config;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Student> _repository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly DbLogger _logger;

        public AuthManager(IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager,
            EmailService emailService, SMTPConfig config, ITokenService tokenService, IConfiguration configuration, 
            IRepository<Student> repository, IRepository<Seller> sellerRepository,
            IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory,
            DbLogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _repository = repository;
            _sellerRepository = sellerRepository;
            _logger = logger;
            var actionContext = actionContextAccessor.ActionContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
            _urlHelper.ActionContext.HttpContext = httpContextAccessor.HttpContext;

        }

        public async Task<AuthResponse> Authenticate([FromBody] LoginRequest request)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new Exception("Пользователь не найден!");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Неверный пароль");
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

            _logger.CreateLog(LogEnum.Information, $"{ipAddress}, вошел в систему как пользователь!");
            return new AuthResponse(200, true, "Операция успешна!"
                , accessToken, user.RefreshToken, user.UserName);
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
            };
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("Пользователь с такой почтой уже зарегистрирован!");
            }
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Произошла некая ошибка при создании нового пользователя!");
            }
            var role = new IdentityRole<int> { Name = "User" };
            var student = new Student { UserId = user.Id };
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, "User");
            await _repository.AddAsync(student);

            return await Authenticate(new LoginRequest
            {
                UserName = request.UserName,
                Password = request.Password
            });
        }

        public async Task<TokenModel> RefreshToken(TokenModel? tokenModel)
        {
            if (tokenModel is null)
                throw new Exception("Ошибка на стороне клиента!");

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;
            var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
                throw new Exception("Неверный \"Access\" или \"Refresh\" токен!");

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Неверный \"Access\" или \"Refresh\" токен!");

            var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
            };
        }

        public async Task<Response> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.RecipientEmail);
            if (user == null)
                throw new Exception("User does not exist.");


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var protocol = _httpContextAccessor.HttpContext.Request.Scheme;
            var callbackUrl = _urlHelper.Action("ResetPassword", "Auth", new { userId = user.Id, token = token }, protocol: protocol);
            var emailRequest = new EmailRequest
            {
                Body = "Администрация Buyursa.kg",
                Subject = $"Вы можете сбросить пароль от вашего аккаунта, перейдя по этой ссылке: <a href='{callbackUrl}'>КЛИК</a>",
                RecipientEmail = request.RecipientEmail
            };
            await _emailService.SendEmailAsync(emailRequest, _config);

            return new Response(200, true, $"{callbackUrl}");
        }

        public async Task<Response> ResetPassword(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User does not exist.");
            }
            var newPass = GeneratePassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);
            var emailRequest = new EmailRequest
            {
                Body = "Администрация Buyursa.kg",
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
            throw new Exception("Failed to reset password.");
        }

        public async Task<UserResponse> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var student = await _repository.GetByUserId(user.Id);

            var seller = await _sellerRepository.GetByUserId(user.Id);
            int studentId = student.Id;
            if (seller == null)
                return new UserResponse(200, true, "Вы успешно вывели текущего пользователя!", user.UserName, user.Email, user.Id, roles.ToList(), studentId, user.IsSeller, 0);

            int sellerId = seller.Id;
            return new UserResponse(200, true, $"Вы успешно вывели текущего пользователя!", user.UserName, user.Email, user.Id, roles.ToList(), studentId, user.IsSeller, sellerId);
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<Response> ChangeCurrentPassword(ChangePasswordRequest request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Неудачная попытка смены пароля, перепроверьте введенные данные!");
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

