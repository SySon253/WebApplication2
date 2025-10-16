using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;

namespace WebApplication2.Components
{
    public class Navbar : ViewComponent
    {
        private readonly WebApplication2Context _context;
        public Navbar(WebApplication2Context context) { _context = context; }
        public IViewComponentResult Invoke()
        {
            return View(_context.Category.ToList());
        }
    }
}
