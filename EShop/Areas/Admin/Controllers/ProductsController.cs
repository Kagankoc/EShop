using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly EShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(EShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //GET /admin/products
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = _context.Products
                .OrderByDescending(x => x.Id)
                .Include(x => x.Category)
                .Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        //GET /admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }

        //POST /admin/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var culture = CultureInfo.CurrentCulture;
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
                return View(product);
            }

            product.Slug = product.Name.ToLower().Replace(" ", "-");


            var slug = await _context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The product already exists");
                return View(product);
            }

            var imageName = "noimage.png";

            if (product.ImageFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                imageName = /*Guid.NewGuid().ToString() + "_" +*/ product.ImageFile.FileName;
                string filePath = Path.Combine(uploadDir, imageName);
                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

                await product.ImageFile.CopyToAsync(fileStream);
                fileStream.Close();

            }
            product.Image = imageName;

            _context.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The product has been added!";

            return RedirectToAction("Index");

        }
        //GET /admin/products/edit/id
        public async Task<IActionResult> Edit(Guid Id)
        {
            Product product = await _context.Products.FindAsync(Id);
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);
            if (product == null)
                return NotFound();

            return View(product);
        }

        //POST /admin/product/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);
            if (ModelState.IsValid)
            {


                product.Slug = product.Name.ToLower().Replace(" ", "-");


                var slug = await _context.Pages.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }


                if (product.ImageFile != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    if (product.Image == null)
                    {
                        product.Image = "noimage.png";
                    }

                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageName = product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

                    await product.ImageFile.CopyToAsync(fileStream);
                    fileStream.Close();
                    product.Image = imageName;

                }


                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been edited!";

                return RedirectToAction("Index");
                //return RedirectToAction("Edit", new { id = product.Id });
            }

            return View(product);

        }
        //GET /admin/product/delete/id
        public async Task<IActionResult> Delete(Guid Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
            {
                TempData["Error"] = "The product does not exist!";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been removed!";
            }


            return RedirectToAction("Index");
        }
        //POST /admin/product/reorder
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
        //GET /admin/products/details/id
        public async Task<IActionResult> Details(Guid Id)
        {
            Product product = await _context.Products.FindAsync(Id);

            if (product == null)
                return NotFound();
            if (product.Category == null)
            {
                product.Category = _context.Categories.Find(product.CategoryId);
                _context.Products.Update(product);
            }
            return View(product);
        }

    }
}
