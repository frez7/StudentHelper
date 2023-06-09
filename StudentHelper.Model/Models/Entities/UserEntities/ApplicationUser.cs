﻿using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsSeller { get; set; } = false;

    }
}
