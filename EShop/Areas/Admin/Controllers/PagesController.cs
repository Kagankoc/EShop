using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly EShopContext _context;

        public PagesController(EShopContext context)
        {
            _context = context;
        }

        //GET admin/pages/
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = _context.Pages.OrderBy(o => o.Sorting).AsQueryable();

            List<Page> pagesList = await pages.ToListAsync();

            return View(pagesList);
        }

        //GET /admin/pages/details/id
        public async Task<IActionResult> Details(Guid Id)
        {
            Page page = await _context.Pages.FirstOrDefaultAsync(page => page.Id == Id);
            if (page == null)
                return NotFound();

            return View(page);
        }

        //GET /admin/pages/create
        public IActionResult Create() => View();

        //POST /admin/pages/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {

                page.Slug = page.Title.ToLower().Replace(" ", "-");

                page.Sorting = 100;

                var slug = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The title already exists");
                    return View(page);
                }

                _context.Add(page);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The page has been added!";

                return RedirectToAction("Index");
            }

            return View(page);

        }
        //GET /admin/pages/edit/id
        public async Task<IActionResult> Edit(Guid Id)
        {
            Page page = await _context.Pages.FindAsync(Id);
            if (page == null)
                return NotFound();

            return View(page);
        }

        //POST /admin/pages/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {

                if (page.Slug != "home")
                {
                    page.Slug = page.Title.ToLower().Replace(" ", "-");
                }






                var slug = await _context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }

                _context.Update(page);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited!";

                return RedirectToAction("Edit", new { id = page.Id });
            }

            return View(page);

        }
        //GET /admin/pages/delete/id
        public async Task<IActionResult> Delete(Guid Id)
        {
            Page page = await _context.Pages.FindAsync(Id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page has been removed!";
            }


            return RedirectToAction("Index");
        }
        //POST /admin/pages/reorder
        [HttpPost]

        public async Task<IActionResult> Reorder(string[] id)
        {

            var count = 1;


            foreach (var pageId in id)
            {
                var itemId = Guid.Parse(pageId.Replace("x", "-"));
                Page page = await _context.Pages.FindAsync(itemId);
                page.Sorting = count;
                _context.Update(page);
                await _context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }

    }

}