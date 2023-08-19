using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class FileController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public FileController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }
    }
}
