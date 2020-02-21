using EShop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly EShopContext _context;

        public ProductsController(EShopContext context)
        {
            _context = context;
        }

        //Get /products
        public async Task<ActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 6;
            var products = _context.Products.OrderByDescending(x => x.Id)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }
        //Get /products/category
        public async Task<ActionResult> ProductsByCategory(string categorySlug, int pageNumber = 1)
        {
            var category = await _context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var pageSize = 6;
            var products = _context.Products.Where(x => x.CategoryId == category.Id).OrderByDescending(x => x.Id)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count(x => x.CategoryId == category.Id) / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;

            return View(await products.ToListAsync());
        }
    }
}