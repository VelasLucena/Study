using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class FileController : Controller
    {
        private readonly ISudyService _sudyService;

        public FileController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }
    }
}
