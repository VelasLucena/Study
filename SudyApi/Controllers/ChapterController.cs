using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;
using System.Collections.Generic;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    [Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ChapterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChapter(int chapterId)
        {
            try
            {
                ChapterModel chapter = await _sudyService.ChapterRepository.GetChapterById(Convert.ToInt32(chapterId));

                if (chapter == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, chapter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet(nameof(List))]
        public async Task<IActionResult> List(int? subjectId, string? name, int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
        {
            try
            {
                _sudyService.DataOptions.KeyOrder = attributeName;
                _sudyService.DataOptions.Take = take;
                _sudyService.DataOptions.Ordering = ordering;

                List<ChapterModel> chapters = new List<ChapterModel>();

                if (subjectId != null)
                    chapters = await _sudyService.ChapterRepository.GetAllChaptersBySubjectId(subjectId.Value);
                else if (!string.IsNullOrEmpty(name))
                    chapters = await _sudyService.ChapterRepository.GetChapterByName(name);
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                if (chapters.Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, chapters);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateChapter(RegisterChapterViewModel chapter)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                _sudyService.DataOptions.IsTracking = true;

                ChapterModel newChapter = chapter;

                await _sudyService.Create(newChapter);

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByChapterId(newChapter.ChapterId);

                discipline.TotalModulesCount += newChapter.ModulesCount;

                await _sudyService.Update(discipline);

                return StatusCode(StatusCodes.Status200OK, newChapter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditChapters(EditChapterViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                _sudyService.DataOptions.IsTracking = true;

                ChapterModel chapterOld = await _sudyService.ChapterRepository.GetChapterById(viewModel.ChapterId);

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByChapterId(chapterOld.ChapterId);

                if (chapterOld.ModulesCount != viewModel.ModulesCount)
                {
                    discipline.TotalModulesCount -= chapterOld.ModulesCount;
                }

                if (chapterOld == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                chapterOld.Update(viewModel);

                await _sudyService.Update(chapterOld);

                if (chapterOld.ModulesCount == viewModel.ModulesCount)
                {
                    discipline.TotalModulesCount += chapterOld.ModulesCount;
                }

                await _sudyService.Update(discipline);

                return StatusCode(StatusCodes.Status200OK, chapterOld);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChapters(int chapterId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                _sudyService.DataOptions.IsTracking = true;

                ChapterModel chapterOld = await _sudyService.ChapterRepository.GetChapterById(chapterId);

                if (chapterOld == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByChapterId(chapterOld.ChapterId);

                discipline.TotalModulesCount -= chapterOld.ModulesCount;

                await _sudyService.Update(discipline);

                await _sudyService.Delete(chapterOld);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
