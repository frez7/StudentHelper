﻿using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Models.Entities;

namespace StudentHelper.WebApi.Service
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, List<IdentityRole<int>> role);
    }
}
