using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [Route("{controller}/{action}")]
    [ApiController]
    public class ConfigSemesterController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ConfigSemesterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(CreateParameter))]
        [Authorize]
        public async Task<IActionResult> CreateParameter(RegisterChapterViewModel[] chapters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                List<ChapterModel> newChapters = new List<ChapterModel>();

                foreach (var chapter in chapters)
                {
                    newChapters.Add(new ChapterModel(chapter));
                }

                await _sudyService.CreateMany(newChapters);

                return Ok(newChapters);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
