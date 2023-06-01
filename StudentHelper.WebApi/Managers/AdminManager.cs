using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Entities;

namespace StudentHelper.WebApi.Managers
{
    public class AdminManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Seller> _sellerRepository;

        public AdminManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IRepository<Student> studentRepository, IRepository<Seller> sellerRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _studentRepository = studentRepository;
            _sellerRepository = sellerRepository;
        }

        public async Task<Response> AddToManager(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var role = new IdentityRole<int>("Manager");
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, "Manager");
            return new Response(200, true, $"Manager был выдан пользователю: {user.UserName}");
        }

        public async Task<Response> AddToAdmin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var role = new IdentityRole<int>("Admin");
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, "Admin");
            return new Response(200, true, $"Admin был выдан пользователю: {user.UserName}");
        }

        public async Task<UserResponse> GetUserByUserName(string username)
        {
            ApplicationUser user;
            IEnumerable<string> roles;
            Student student = new Student();
            int studentId = 0;
            int sellerId = 0;
            Seller seller = new Seller();
            try
            {
                user = await _userManager.FindByNameAsync(username);
                roles = await _userManager.GetRolesAsync(user);
                student = await _studentRepository.GetByUserId(user.Id);
                studentId = student.Id;
                if (seller == null)
                {
                    return new UserResponse(200, true, "Вы успешно нашли юзера!", user.UserName, user.Email, user.Id, roles.ToList(), studentId, user.IsSeller, 0);
                }

            }
            catch (Exception)
            {
                return new UserResponse(400, false, "Пользователь с таким никнеймом не найден!", null, null, 0, null, 0, false, 0);
            }

            return new UserResponse(200, true, "User has been finded", user.UserName, user.Email, user.Id, roles.ToList(), studentId, user.IsSeller, sellerId);
        }
    }
}

