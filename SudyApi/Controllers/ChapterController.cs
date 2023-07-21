using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class ChapterController : Controller
    {
        private readonly ISudyService _sudyService;

        public ChapterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllChapters))]
        [Authorize]
        public async Task<IActionResult> GetAllChapters(int subjectId)
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

        [HttpGet]
        [ActionName(nameof(GetChapter))]
        [Authorize]
        public async Task<IActionResult> GetChapter(int chapterId)
        {
            try
            {
                ChapterModel chapter = await _sudyService.ChapterRepository.GetChapterByIdNoTracking(Convert.ToInt32(chapterId));

                if (chapter == null)
                    return NotFound();

                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetChapterListByName))]
        [Authorize]
        public async Task<IActionResult> GetChapterListByName(string name)
        {
            try
            {
                List<ChapterModel> chapters = await _sudyService.ChapterRepository.GetChapterByNameNoTracking(name);

                if (chapters.Count == 0)
                    return NotFound();

                return Ok(chapters);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateChapters))]
        [Authorize]
        public async Task<IActionResult> CreateChapters(RegisterChapterViewModel[] chapters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                List<ChapterModel> newChapters = new List<ChapterModel>();

                foreach(var chapter in chapters)
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

        [HttpPut]
        [ActionName(nameof(EditChapters))]
        [Authorize]
        public async Task<IActionResult> EditChapters(List<EditChapterViewModel> chapters)
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

        [HttpDelete]
        [ActionName(nameof(DeleteChapters))]
        [Authorize]
        public async Task<IActionResult> DeleteChapters(List<DeleteChaptersViewModel> chapters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                List<ChapterModel> deleteChapters = new List<ChapterModel>();

                foreach (DeleteChaptersViewModel item in chapters)
                {
                    ChapterModel chapter = await _sudyService.ChapterRepository.GetChapterById(item.ChapterId);

                    if (chapter == null)
                        return NotFound();

                    deleteChapters.Add(chapter);
                }

                await _sudyService.DeleteMany(deleteChapters);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
