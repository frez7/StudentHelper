using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;
using System.Web.Http;

namespace StudentHelper.BL.Services.CourseServices
{
    public class PageService
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Course> _courseRepository; 
        private readonly CourseContext _context;
        private readonly ValidationService _validationService;
        private readonly GetService _getService;

        public PageService(IRepository<Page> pageRepository, CourseContext context, ValidationService validationService
            , GetService getService, IRepository<Course> courseRepository)
        {
            _pageRepository = pageRepository;
            _context = context;
            _validationService = validationService;
            _getService = getService;
            _courseRepository = courseRepository;
        }
        public async Task<List<PageDTO>> GetAllPagesByCourseId(int courseId)
        {
            var validity = await _validationService.CheckCourseRelation(courseId);
            if (validity == false)
            {
                throw new Exception("Вы не приобрели данный курс!");
            }
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
        public async Task<PageDTOResponse> GetPageById(int pageId)
        {
            var page = await _pageRepository.GetByIdAsync(pageId);
            if (page == null)
            {
                throw new Exception("Страница с таким айди не найдена!");
            }
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            
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
            var validSeller = await _validationService.GetPageOwner(pageId);
            if (validSeller == false)
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

            var validSeller = await _validationService.GetPageOwner(request.PageId);
            if (validSeller == false)
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
            var id = _getService.GetCurrentUserId();
            var validSeller = await _validationService.GetCourseOwner(request.CourseId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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
