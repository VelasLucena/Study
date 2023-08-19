using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;
using System.Net;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    [Authorize]
    public class SemesterController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public SemesterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet(nameof(List))]
        public async Task<IActionResult> List(int userId, int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
        {
            try
            {
                _sudyService.DataOptions.KeyOrder = attributeName;
                _sudyService.DataOptions.Take = take;
                _sudyService.DataOptions.Ordering = ordering;

                List<SemesterModel> semesters = await _sudyService.SemesterRepository.GetAllSemestersByUserId(userId);

                if (semesters.Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, semesters);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSemester(int semesterId)
        {
            try
            {
                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(semesterId);

                if (semester == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, semester);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSemester(RegisterSemesterModel semester)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                _sudyService.DataOptions.IsTracking = true;

                UserModel user = await _sudyService.UserRepository.GetUserById(semester.UserId);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                CourseModel course = await _sudyService.CourseRepository.GetCourseById(semester.CourseId);

                if (course == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                InstitutionModel institution = await _sudyService.InstitutionRepository.GetInstitutionById(semester.InstitutionId);

                if (institution == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                SemesterModel newSemester = new SemesterModel(semester, user, course, institution);

                await _sudyService.Create(newSemester);

                return StatusCode(StatusCodes.Status200OK, newSemester);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditSemester(EditSemesterViewModel semester)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                _sudyService.DataOptions.IsTracking = true;

                SemesterModel editSemester = await _sudyService.SemesterRepository.GetSemesterById(semester.SemesterId);

                UserModel user = await _sudyService.UserRepository.GetUserById(UserLogged.UserId);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                CourseModel course = await _sudyService.CourseRepository.GetCourseById(Convert.ToInt32(semester.CourseId));

                if (course == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                InstitutionModel institution = await _sudyService.InstitutionRepository.GetInstitutionById(Convert.ToInt32(semester.InstitutionId));

                if (institution == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                editSemester.Update(semester, user, course, institution);

                await _sudyService.Update(editSemester);

                return StatusCode(StatusCodes.Status200OK, editSemester);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSemester(int semesterId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(semesterId);

                if (semester == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                List<DisciplineModel> disciplines = await _sudyService.DisciplineRepository.GetDisciplinesBySemesterId(semesterId);

                List<SubjectModel> subjects = new List<SubjectModel>();

                List<ChapterModel> chapters = new List<ChapterModel>();

                if (disciplines.Count > 0)
                {
                    foreach (DisciplineModel item in disciplines)
                    {
                        subjects.AddRange(await _sudyService.SubjectRepository.GetSubjectByDisciplineId(item.DisciplineId));
                    }

                    if (subjects.Count > 0)
                    {
                        foreach (SubjectModel item in subjects)
                        {
                            chapters.AddRange(await _sudyService.ChapterRepository.GetAllChaptersBySubjectId(item.SubjectId));
                        }

                        if (chapters.Count > 0)
                            await _sudyService.Delete(chapters);

                        await _sudyService.Delete(subjects);
                    }

                    await _sudyService.Delete(disciplines);
                }

                await _sudyService.Delete(semester);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
