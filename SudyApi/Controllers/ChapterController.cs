using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class ChapterController
    {
        private readonly ISudyService _sudyService;

        public ChapterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }
    }
}
