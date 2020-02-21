using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;


        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
    }
}