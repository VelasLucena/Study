using Microsoft.AspNetCore.Mvc;

namespace StudandoApi.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
