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
    public class SemesterController : Controller
    {
        private readonly ISudyService _sudyService;

        public SemesterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllSemestersByUserId))]
        [Authorize]
        public async Task<IActionResult> GetAllSemestersByUserId(int userId)
        {
            try
            {
                List<SemesterModel> semesters = await _sudyService.SemesterRepository.GetAllSemestersByUserIdNoTracking(userId);

                if (semesters.Count == 0)
                    return NotFound();

                return Ok(semesters);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [ActionName(nameof(GetSemester))]
        [Authorize]
        public async Task<IActionResult> GetSemester(int semesterId)
        {
            try
            {
                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterByIdNoTracking(semesterId);

                if (semester == null)
                    return NotFound();

                return Ok(semester);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateSemester))]
        [Authorize]
        public async Task<IActionResult> CreateSemester(RegisterSemesterModel semester)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                UserModel user = await _sudyService.UserRepository.GetUserById(semester.UserId);

                if (user == null)
                    return NotFound();

                CourseModel course = await _sudyService.CourseRepository.GetCourseById(semester.CourseId);

                if(course == null)
                    return NotFound();

                InstitutionModel institution = await _sudyService.InstitutionRepository.GetInstitutionById(semester.InstitutionId);

                if (institution == null)
                    return NotFound();

                SemesterModel newSemester = new SemesterModel(semester, user, course, institution);

                await _sudyService.Create(newSemester);

                return Ok(newSemester);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [ActionName(nameof(EditSemester))]
        [Authorize]
        public async Task<IActionResult> EditSemester(EditSemesterViewModel semester)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                SemesterModel editSemester = await _sudyService.SemesterRepository.GetSemesterById(semester.SemesterId);

                UserModel user = await _sudyService.UserRepository.GetUserById(UserLogged.UserId);

                if (user == null)
                    return NotFound();

                CourseModel course = await _sudyService.CourseRepository.GetCourseById(Convert.ToInt32(semester.CourseId));

                if (course == null)
                    return NotFound();

                InstitutionModel institution = await _sudyService.InstitutionRepository.GetInstitutionById(Convert.ToInt32(semester.InstitutionId));

                if (institution == null)
                    return NotFound();

                editSemester.Update(semester, user, course, institution);

                await _sudyService.Update(editSemester);

                return Ok(editSemester);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteSemester))]
        [Authorize]
        public async Task<IActionResult> DeleteSemester(int semesterId)
        {
            try
            {
                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(semesterId);

                if (semester == null)
                    return NotFound();

                List<DisciplineModel> disciplines = await _sudyService.DisciplineRepository.GetDisciplinesBySemesterId(semesterId);

                List<SubjectModel> subjects = new List<SubjectModel>();

                List<ChapterModel> chapters = new List<ChapterModel>();

                if (disciplines.Count > 0)
                {
                    foreach (DisciplineModel item in disciplines)
                    {
                        subjects.AddRange(await _sudyService.SubjectRepository.GetSubjectByDisciplineId(item.DisciplineId));
                    }

                    if(subjects.Count > 0)
                    {
                        foreach(SubjectModel item in subjects)
                        {
                            chapters.AddRange(await _sudyService.ChapterRepository.GetAllChaptersBySubjectId(item.SubjectId));
                        }

                        if (chapters.Count > 0)
                            await _sudyService.DeleteMany(chapters);

                        await _sudyService.DeleteMany(subjects);
                    }

                    await _sudyService.DeleteMany(disciplines);
                }

                await _sudyService.Delete(semester);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
