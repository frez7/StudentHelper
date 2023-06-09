﻿using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Course : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageURL { get; set; }
        public Seller Seller { get; set; }
        public int SellerId { get; set; }
        public List<StudentCourse>? Students { get; set; }
        public List<Page>? Pages { get; set; }
    }
}
