using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly EShopContext _context;

        public CategoriesController(EShopContext context)
        {
            _context = context;
        }

        //GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        //GET /admin/categories/create
        public IActionResult Create() => View();
        //POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            category.Slug = category.Name.ToLower().Replace(" ", "-");

            category.Sorting = 100;

            var slug = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The name already exists");
                return View(category);
            }

            _context.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The category has been added!";

            return RedirectToAction("Index");

        }
        //GET /admin/categories/edit/id
        public async Task<IActionResult> Edit(Guid Id)
        {
            Category category = await _context.Categories.FindAsync(Id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        //POST /admin/categories/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            if (ModelState.IsValid)
            {


                category.Slug = category.Name.ToLower().Replace(" ", "-");


                var slug = await _context.Pages.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }

                _context.Update(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The category has been edited!";

                return RedirectToAction("Edit", new { id = category.Id });
            }

            return View(category);

        }
        //GET /admin/categories/delete/id
        public async Task<IActionResult> Delete(Guid Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exist!";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The category has been removed!";
            }


            return RedirectToAction("Index");
        }
        //POST /admin/categories/reorder
        [HttpPost]

        public async Task<IActionResult> Reorder(string[] id)
        {

            var count = 1;


            foreach (var pageId in id)
            {
                var itemId = Guid.Parse(pageId.Replace("x", "-"));
                var category = await _context.Categories.FindAsync(itemId);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }
    }
}