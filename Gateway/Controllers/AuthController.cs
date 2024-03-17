using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
