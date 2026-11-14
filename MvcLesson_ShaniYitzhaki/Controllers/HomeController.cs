using Microsoft.AspNetCore.Mvc;

namespace MvcLesson_ShaniYitzhaki.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
