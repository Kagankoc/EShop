using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly EShopContext _context;

        public CategoriesViewComponent(EShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return _context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
