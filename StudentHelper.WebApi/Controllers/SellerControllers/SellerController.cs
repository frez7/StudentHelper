﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common.SellerResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Queries.SellerQueries;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly SellerService _sellerService;
        private readonly EnrollmentService _enrollmentService;
        private readonly IMediator _mediator;

        public SellerController(SellerService sellerService, EnrollmentService enrollmentService, IMediator mediator)
        {
            _sellerService = sellerService;
            _enrollmentService = enrollmentService;
            _mediator = mediator;
        }

        [HttpGet("sellers")]
        public async Task<List<Seller>> GetSellersAsync()
        {
            return await _sellerService.GetSellersAsync();
        }

        [HttpGet("seller/enrollments")]
        public async Task<EnrollmentsResponse> GetSellerEnrollments()
        {
            return await _enrollmentService.GetSellerEnrollments();
        }

        [HttpGet("Get-Seller-By-Id")]
        public async Task<SellerDTO> GetSellerById(int id)
        {
            return await _mediator.Send(new GetSellerByIdQuery { Id = id });
        }
        [HttpGet("seller/courses")]
        public async Task<List<CourseDTO>> GetAllSellerCourses()
        {
            return await _sellerService.GetAllSellerCourses();
        }
    }
}
