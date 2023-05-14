using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Models.Entities;

namespace StudentHelper.WebApi.Data
{
    public static class UserRoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Manager", "User" };
            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            var email = "admin@mail.ru";
            var password = "qwerty";
            var username = "qwerty";

            if (userManager.FindByEmailAsync(email).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = email,
                    UserName = username,
                };

                IdentityResult userResult = userManager.CreateAsync(user, password).Result;

                if (userResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            
        }
    }

}
