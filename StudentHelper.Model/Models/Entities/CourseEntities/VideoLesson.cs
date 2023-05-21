﻿using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class VideoLesson : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string VideoUrl { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
    }
}