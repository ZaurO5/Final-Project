using Business.ViewModels.User;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                .Select(u => new UserListVM
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed
                })
                .ToList();

            return View(users);
        }
    }
}
