using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
namespace WebApplication2.Components
{
    public class Imagebar : ViewComponent
    {
        private readonly WebApplication2Context _context;
        public Imagebar(WebApplication2Context context) { _context = context; }
        public IViewComponentResult Invoke()
        {
            return View("Index", _context.Category.ToList());
        }
    }
}
