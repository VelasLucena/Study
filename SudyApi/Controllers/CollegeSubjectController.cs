using Microsoft.AspNetCore.Mvc;

namespace SudyApi.Controllers
{
    public class CollegeSubjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
