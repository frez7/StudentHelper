﻿using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Models.Entities.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
        public int SellerId { get; set; }
        public List<PageDTO> Pages {  get; set; }

    }
}
