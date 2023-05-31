﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services.CourseServices
{
    public class VideoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<VideoLesson> _videoLessonRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly CourseContext _dbContext;

        public VideoService(IHttpContextAccessor httpContextAccessor, IRepository<VideoLesson> videoLessonRepository,
            IRepository<Course> courseRepository, IRepository<Seller> sellerRepository, IRepository<Page> pageRepository
            ,CourseContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _videoLessonRepository = videoLessonRepository;
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _pageRepository = pageRepository;
            _dbContext = dbContext;
        }
        public async Task<VideoResponse> AddVideoLesson([FromBody] AddVideoLessonRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            Seller seller = await _sellerRepository.GetByUserId(id);
            var page = await _pageRepository.GetByIdAsync(request.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (page == null)
            {
                throw new Exception("Страница с таким айди не найдена!");
            }
            if (seller == null)
            {
                throw new Exception("Ты не являешься продавцом!");
            }
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Этот курс не пренадлежит тебе, ты не можешь добавлять в него видеоуроки!");
            }
            var videoLesson = new VideoLesson
            {
                Description = request.Description,
                PageId = request.PageId,
                Title = request.Title,
                VideoUrl = request.VideoUrl,
            };
            await _videoLessonRepository.AddAsync(videoLesson);
            return new VideoResponse(200, true, "Вы успешно добавили видео урок!", videoLesson.Id);
        }

        public async Task<List<VideoLessonDTO>> GetVideosByPageId(int pageId)
        {
        var videos = _dbContext.VideoLessons.Where(v => v.PageId == pageId).ToList();
        var videosDto = new List<VideoLessonDTO>();
        for (int i = 0; i < videos.Count; i++)
        {
            var videoDto = new VideoLessonDTO()
            {
                Description = videos[i].Description,
                Id = videos[i].Id,
                PageId = videos[i].PageId,
                Title = videos[i].Title,
                VideoUrl = videos[i].VideoUrl,
            };
            videosDto.Add(videoDto);
        }
        return videosDto;
        }
        public async Task<VideoLessonDTO> GetVideoById(int videoId)
        {
            var videoLesson = await _videoLessonRepository.GetByIdAsync(videoId);
            if (videoLesson == null)
            {
                throw new Exception("Видео с таким айди не найдено!");
            }
            var videoDto = new VideoLessonDTO
            {
                Description = videoLesson.Description,
                Title = videoLesson.Title,
                Id = videoLesson.Id,
                PageId = videoLesson.PageId,
                VideoUrl = videoLesson.VideoUrl,
            };
            return videoDto;
        }
        public async Task<VideoResponse> UpdateVideo(int videoId, [FromBody] AddVideoLessonRequest request)
        {
            var video = await _videoLessonRepository.GetByIdAsync(videoId);
            if (video == null)
            {
                throw new Exception("Страница не найдена..");
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var page = await _pageRepository.GetByIdAsync(request.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }
            video.Title = request.Title;
            video.Description = request.Description;
            video.VideoUrl = request.VideoUrl;
            video.PageId = request.PageId;
            await _videoLessonRepository.UpdateAsync(video);
            return new VideoResponse(200, true, "Видео обновлено!", videoId);
        }
        public async Task<Response> DeleteVideo(int videoId)
        {
            var video = await _videoLessonRepository.GetByIdAsync(videoId);
            if (video == null)
            {
                throw new Exception("Данного видео не существует!");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var page = await _pageRepository.GetByIdAsync(video.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }
            await _videoLessonRepository.DeleteAsync(videoId);
            return new Response(200, true, "Видео успешно удалено!");
        }
    }
}