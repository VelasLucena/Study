using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudandoApi.Data.Interfaces;
using StudandoApi.Models.User;
using SudyApi.Models.Subject;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class SubjectController : Controller
    {
        private readonly ISudyService _sudyService;

        public SubjectController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllSubjects))]
        [Authorize]
        public async Task<IActionResult> GetAllSubjects(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(SubjectModel.SubjectId))
        {
            try
            {
                List<SubjectModel> subjects = await _sudyService.SubjectRepository.GetAllSubjectsNoTracking(limit, ordering, attributeName);

                if (subjects.Count == 0)
                    return NotFound();

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [ActionName(nameof(GetSubject))]
        [Authorize]
        public async Task<IActionResult> GetSubject (int? subjectId, string? name)
        {
            try
            {
                SubjectModel subject = new SubjectModel();

                if (subjectId.HasValue)
                    subject = await _sudyService.SubjectRepository.GetSubjectBySubjectIdNoTracking(Convert.ToInt32(subjectId));
                else if (string.IsNullOrEmpty(name))
                    subject = await _sudyService.SubjectRepository.GetSubjectByNameFirstNoTracking(name);
                else
                    return BadRequest();

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateSubject))]
        public async Task<IActionResult> CreateSubject(RegisterSubjectViewModel subject)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                UserModel user = await _sudyService.UserRepository.GetUserById(subject.UserId);

                if(user == null)
                    return NotFound();

                SubjectModel newSubject = new SubjectModel(subject, user);

                await _sudyService.Create(newSubject, true);

                List<ChapterModel> chapters = new List<ChapterModel>();

                foreach(RegisterChapterViewModel item in subject.Chapters)
                {
                    chapters.Add(new ChapterModel(item, newSubject));
                }

                await _sudyService.CreateMany(chapters, true);

                SubjectModel subjectView = await _sudyService.SubjectRepository.GetSubjectBySubjectIdNoTracking(newSubject.SubjectId);

                return Ok(subjectView);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [ActionName(nameof(EditSubject))]
        [Authorize]
        public async Task<IActionResult> EditSubject(EditSubjectViewModel subject)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                UserModel user = await _sudyService.UserRepository.GetUserByIdNoTracking(Convert.ToInt32(subject.UserId));

                if (user == null)
                    return NotFound();

                List<ChapterModel> chaptersOld = await _sudyService.ChapterRepository.GetAllChaptersBySubjectIdNoTracking(subject.SubjectId);
                List<int> subjectChaptersList = subject.Chapters.Select(x => x.ChapterId).ToList();
                List<ChapterModel> deleteChapter = new List<ChapterModel>();
                foreach (var chapter in chaptersOld)
                {
                    if (!subjectChaptersList.Contains(chapter.ChapterId))
                        deleteChapter.Add(chapter);
                }
                await _sudyService.DeleteMany(deleteChapter);

                SubjectModel editSubject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subject.SubjectId);
                editSubject.Update(subject);
                await _sudyService.Update(editSubject, true);

                List<ChapterModel> chapters = new List<ChapterModel>();
                foreach (EditChapterViewModel item in subject.Chapters)
                {
                    ChapterModel chapter = await _sudyService.ChapterRepository.GetChapterById(item.ChapterId);
                    chapter.Update(item);
                    chapters.Add(chapter);
                }
                await _sudyService.UpdateMany(chapters, true);

                SubjectModel subjectView = await _sudyService.SubjectRepository.GetSubjectBySubjectIdNoTracking(subject.SubjectId);

                return Ok(subjectView);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteSubject))]
        [Authorize]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            try
            {
                SubjectModel subject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subjectId);

                if(subject == null)
                    return NotFound();

                List<ChapterModel> chapters = await _sudyService.ChapterRepository.GetAllChaptersBySubjectId(subjectId);

                if (chapters.Count == 0)
                    return NotFound();

                await _sudyService.Delete(subject);
                await _sudyService.DeleteMany(chapters);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
