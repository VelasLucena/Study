using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class ImportantDatesController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ImportantDatesController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(CreateImportantDate))]
        [Authorize]
        public async Task<IActionResult> CreateImportantDate(RegisterImportanteDateViewModel importanteDate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                ImportantDateModel newImportantDate = new ImportantDateModel(importanteDate);

                await _sudyService.Create(newImportantDate);

                return Ok(newImportantDate);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [ActionName(nameof(EditImportantDate))]
        [Authorize]
        public async Task<IActionResult> EditImportantDate(List<EditChapterViewModel> chapters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                List<ChapterModel> editChapters = new List<ChapterModel>();

                foreach (var chapter in chapters)
                {
                    ChapterModel chapterOld = await _sudyService.ChapterRepository.GetChapterById(chapter.ChapterId);

                    if (chapterOld == null)
                        return NotFound();

                    chapterOld.Update(chapter);

                    editChapters.Add(chapterOld);
                }

                await _sudyService.UpdateMany(editChapters);

                return Ok(editChapters);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
