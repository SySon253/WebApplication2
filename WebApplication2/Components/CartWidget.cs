using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Infrastructure;

namespace WebApplication2.Components
{
    public class CartWidget : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
