﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests
{
    public class PaymentForCourseRequest
    {
        [Required]
        public int CourseId { get; set; }

    }
}