﻿using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;
using System.Security.Claims;
using System.Web.Http;

namespace StudentHelper.BL.Services.CourseServices
{
    public class PageService
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CourseContext _context;

        public PageService(IRepository<Page> pageRepository, IRepository<Seller> sellerRepository, IRepository<Course> courseRepository, CourseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _pageRepository = pageRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<PageDTO>> GetAllPagesByCourseId(int courseId)
        {
            var pages = _context.Pages.Where(p => p.CourseId == courseId).ToList();
            var pagesDto = new List<PageDTO>();
            
            for (int i = 0; i < pages.Count; i++)
            {
                var pageDto = new PageDTO()
                {
                    Content = pages[i].Content,
                    CourseId = pages[i].CourseId,
                    Description = pages[i].Description,
                    Id = pages[i].Id,
                    Title = pages[i].Title,
                };
                pagesDto.Add(pageDto);
            }
            return pagesDto;
        }
        public async Task<PageDTOResponse> GetPageById( int pageId)
        {
            var page = _context.Pages.FirstOrDefault(p => p.Id == pageId);
            if (page == null)
            {
                throw new Exception("Страница с таким айди не найдена!");
            }
            var pageDto = new PageDTO
            {
                Id = pageId,
                Title = page.Title,
                Description = page.Description,
                Content = page.Content,
                CourseId = page.CourseId,
            };
            return new PageDTOResponse(200, true, null, pageDto);
        }
        public async Task<Response> DeletePageById(int pageId)
        {
            var page = await _pageRepository.GetByIdAsync(pageId);
            if (page == null)
            {
                throw new Exception("Страница с таким айди не найдена");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }

            await _pageRepository.DeleteAsync(pageId);
            return new Response(200, true, "Страница у курса успешно удалена!");
        }
        public async Task<PageResponse> UpdatePage(UpdatePageRequest request)
        {
            var page = await _pageRepository.GetByIdAsync(request.PageId);
            if (page ==null)
            {
                throw new Exception("Страница не найдена..");
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }

            page.Title = request.Title;
            page.Description = request.Description;
            page.Content = request.Content;
            await _pageRepository.UpdateAsync(page);
            return new PageResponse(200, true, "Страница обновлена!", page.Id);
        }
        public async Task<PageResponse> CreatePage([FromBody] CreatePageRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            Seller seller = await _sellerRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new PageResponse(400, false, "Курс с таким айди не найден!", 0);
            }
            if (seller == null)
            {
                return new PageResponse(400, false, "Ты не являешься продавцом!", 0);
            }
            if (seller.Id != course.SellerId)
            {
                return new PageResponse(400, false, "Этот курс не пренадлежит тебе, ты не можешь добавлять в него страницы!",0);
            }
            var page = new Page
            {
                Title = request.Title,
                Description = request.Description,
                Content = request.Content,
                CourseId = request.CourseId,
            };
            await _pageRepository.AddAsync(page);
            return new PageResponse(200, true, "Страница добавлена!", page.Id);
        }
    }
}
