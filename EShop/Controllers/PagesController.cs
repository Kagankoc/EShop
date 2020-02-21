using EShop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class PagesController : Controller
    {
        private readonly EShopContext _context;

        public PagesController(EShopContext context)
        {
            _context = context;
        }

        //Get /or /Slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await _context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }

            var page = await _context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}