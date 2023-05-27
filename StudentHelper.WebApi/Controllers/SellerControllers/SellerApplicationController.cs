
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Enums;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Configs;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests;
using StudentHelper.Model.Models.Requests.SellerRequests;
using StudentHelper.WebApi.Service;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerApplicationController : ControllerBase
    {
        private readonly IRepository<SellerApplication> _sellerAppRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly EmailService _emailService;
        private readonly SMTPConfig _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerApplicationController(IRepository<SellerApplication> sellerAppRepository, IRepository<Seller> sellerRepository, EmailService emailService, SMTPConfig config, UserManager<ApplicationUser> userManager)
        {
            _sellerAppRepository = sellerAppRepository;
            _sellerRepository = sellerRepository;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
        }
        [HttpGet("seller-app/{applicationId}")]
        public async Task<SellerApplication> GetById(int applicationId)
        {
            var sellerApp = await _sellerAppRepository.GetByIdAsync(applicationId);
            return sellerApp;
        }
        [HttpPost("create-seller-application")]
        public async Task<Response> CreateSellerApplication(SellerApplicationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

        [HttpGet("status")]
        public async Task<SellerApplicationResponse> GetSelfStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userId, out var id);
            var sellerApplication = await _sellerAppRepository.GetByUserId(id);

            if (sellerApplication == null)
            {
                return new SellerApplicationResponse(404, false, "Заявка не найдена!", null);
            }

            return new SellerApplicationResponse(200, true, "Заявка найдена!", sellerApplication.Status.ToString());
        }

        [HttpGet("get-all-applications")]
        public async Task<List<SellerApplication>> GetAllSellerApplications()
        {
            var sellerApplications = await _sellerAppRepository.GetAllAsync();
            return sellerApplications.ToList();
        }

        [HttpPost("{id}/approve")]
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
                Body = "Администрация ChillCourse",
                Subject = "Ваша заявка была обработана! Вас успешно утвердили в роли продавца! Хороших продаж!"
            };
            await _sellerRepository.AddAsync(seller);
            await _emailService.SendEmailAsync(emailRequest, _config);
            return new Response(200, true, "Вы успешно подтвердили заявку на продавца!");
        }

        [HttpPost("{id}/reject")]
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
