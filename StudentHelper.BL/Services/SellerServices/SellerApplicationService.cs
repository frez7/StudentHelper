using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Enums;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Requests.SellerRequests;
using StudentHelper.Model.Models.Requests;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StudentHelper.BL.Services.OtherServices;

namespace StudentHelper.BL.Services.SellerServices
{
    public class SellerApplicationService
    {
        private readonly IRepository<SellerApplication> _sellerAppRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly EmailService _emailService;
        private readonly SMTPConfig _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellerApplicationService(IRepository<SellerApplication> sellerAppRepository, IRepository<Seller> sellerRepository, EmailService emailService, SMTPConfig config, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _sellerAppRepository = sellerAppRepository;
            _sellerRepository = sellerRepository;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SellerApplication> GetById(int applicationId)
        {
            var sellerApp = await _sellerAppRepository.GetByIdAsync(applicationId);
            return sellerApp;
        }
        public async Task<Response> CreateSellerApplication(SellerApplicationRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var existingApp = await _sellerAppRepository.GetByUserId(id);
            if (existingApp != null)
            {
                return new Response(400, false, "Вы уже отправляли заявку на продавца!");
            }
            var sellerApplication = new SellerApplication
            {
                UserId = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                CompanyName = request.CompanyName,
                CompanyDescription = request.CompanyDescription,
                Status = SellerApplicationStatus.NotReviewed,
                CreatedAt = DateTime.UtcNow,
            };
            await _sellerAppRepository.AddAsync(sellerApplication);

            return new Response(200, true, "Заявка на становление продавцом успешно отправлена!");
        }

        public async Task<SellerApplicationResponse> GetSelfStatus()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var sellerApplication = await _sellerAppRepository.GetByUserId(id);

            if (sellerApplication == null)
            {
                return new SellerApplicationResponse(404, false, "Заявка не найдена!", null);
            }

            return new SellerApplicationResponse(200, true, "Заявка найдена!", sellerApplication.Status.ToString());
        }

        public async Task<List<SellerApplication>> GetAllSellerApplications()
        {
            var sellerApplications = await _sellerAppRepository.GetAllAsync();
            return sellerApplications.ToList();
        }

        public async Task<Response> Approve(int id)
        {
            var sellerApplication = await _sellerAppRepository.GetByIdAsync(id);

            if (sellerApplication == null)
            {
                return new Response(404, false, "Заявка не найдена!");
            }
            if (sellerApplication.Status == SellerApplicationStatus.Approved)
            {
                return new Response(400, false, "Данная заявка уже подтверждена!");
            }

            sellerApplication.Status = SellerApplicationStatus.Approved;
            await _sellerAppRepository.UpdateAsync(sellerApplication);
            var user = await _userManager.FindByIdAsync($"{sellerApplication.UserId}");
            user.IsSeller = true;
            await _userManager.UpdateAsync(user);
            var seller = new Seller
            {
                UserId = sellerApplication.UserId,
                IsConfirmed = true,
                FirstName = sellerApplication.FirstName,
                LastName = sellerApplication.LastName,
                Email = sellerApplication.Email,
                Phone = sellerApplication.Phone,
                CompanyName = sellerApplication.CompanyName,
                CompanyDescription = sellerApplication.CompanyDescription,
            };
            var emailRequest = new EmailRequest
            {
                RecipientEmail = sellerApplication.Email,
                Body = "Администрация Buyursa.kg",
                Subject = "Ваша заявка была обработана! Вас успешно утвердили в роли продавца! Хороших продаж!"
            };
            await _sellerRepository.AddAsync(seller);
            await _emailService.SendEmailAsync(emailRequest, _config);
            return new Response(200, true, "Вы успешно подтвердили заявку на продавца!");
        }

        public async Task<Response> Reject(int id)
        {
            var sellerApplication = await _sellerAppRepository.GetByIdAsync(id);

            if (sellerApplication == null)
            {
                return new Response(400, false, "Заявка с таким id не найдена!");
            }

            sellerApplication.Status = SellerApplicationStatus.Rejected;
            await _sellerAppRepository.UpdateAsync(sellerApplication);

            return new Response(200, true, "Вы успешно отказали!");
        }
    }
}
