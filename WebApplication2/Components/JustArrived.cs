using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;

namespace WebApplication2.Components
{
    public class JustArrived : ViewComponent
    {
        private readonly WebApplication2Context _context;
        public JustArrived(WebApplication2Context context) { _context = context; }
        public IViewComponentResult Invoke()
        {
            return View(_context.Product.Where(p => p.IsArrived == true).ToList());
        }
    }
}
