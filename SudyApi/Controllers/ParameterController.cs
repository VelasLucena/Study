using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class ParameterController : Controller
    {
        private readonly ISudyService _sudyService;

        public ParameterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(CreateParameter))]
        [Authorize]
        public async Task<IActionResult> CreateParameter(int subjectId)
        {
            try
            {
                List<ChapterModel> chapters = await _sudyService.ChapterRepository.GetAllChaptersBySubjectIdNoTracking(subjectId);

                if (chapters.Count == 0)
                    return NotFound();

                return Ok(chapters);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
