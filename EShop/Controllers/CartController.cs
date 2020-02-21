using EShop.Infrastructure;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class CartController : Controller
    {
        private readonly EShopContext _context;

        public CartController(EShopContext context)
        {
            _context = context;
        }

        //Get /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartViewModel = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            return View(cartViewModel);
        }

        // Get /cart/add/id
        public async Task<IActionResult> Add(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return ViewComponent("SmallCart");
            }


        }

        // Get /cart/decrease/id
        public IActionResult Decrease(Guid id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            var cartItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }



            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }


            return RedirectToAction("Index");
        }

        // Get /cart/remove/id
        public IActionResult Remove(Guid id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");


            cart.RemoveAll(x => x.ProductId == id);




            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }


            return RedirectToAction("Index");
        }

        // Get /cart/clear
        public IActionResult Clear(Guid id)
        {
            HttpContext.Session.Remove("Cart");

            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }
    }
}