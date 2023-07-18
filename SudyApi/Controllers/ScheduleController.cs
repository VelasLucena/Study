using Microsoft.AspNetCore.Mvc;

namespace SudyApi.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
