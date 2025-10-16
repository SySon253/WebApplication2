using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Infrastructure;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly WebApplication2Context _context;

        public CartController(WebApplication2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            //return View("Cart", Cart);
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }
        public IActionResult AddToCart(int productId)
        {
            Product? product = _context.Product.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
            //// Load cả Size và Color của sản phẩm
            //var product = _context.Product
            //    .Include(p => p.Size)
            //    .Include(p => p.Color)
            //    .FirstOrDefault(p => p.ProductId == productId);

            //if (product != null)
            //{
            //    Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            //    Cart.AddItem(product, 1);
            //    HttpContext.Session.SetJson("cart", Cart);
            //}

            //return View("Cart", Cart);
        }
        public IActionResult UpdateCart(int productId)
        {
            Product? product = _context.Product.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Product.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
    }
}
