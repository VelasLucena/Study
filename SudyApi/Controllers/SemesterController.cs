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
    [Route("{controller}/{action}")]
    public class SemesterController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public SemesterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetSemestersList))]
        [Authorize]
        public async Task<IActionResult> GetSemestersList(int userId, int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
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
        [ActionName(nameof(GetSemester))]
        [Authorize]
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
        [ActionName(nameof(CreateSemester))]
        [Authorize]
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

        [HttpPost]
        [ActionName(nameof(CreateScheduleSemester))]
        [Authorize]
        public async Task<IActionResult> CreateScheduleSemester(int semesterId)
        {
            try
            {
                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(semesterId);

                if (!SemesterModel.ScheduleIsPossible(semester))
                {
                    int hourForStudyPossible;

                    for (int y = 1; y <= 5; y++)
                    {
                        semester.ConfigSemester.HoursForStudy = y;

                        if (SemesterModel.ScheduleIsPossible(semester))
                        {
                            hourForStudyPossible = y;

                            await _sudyService.Update(semester);
                        }
                    }

                    if (semester.ConfigSemester.HoursForStudy == null)
                        return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );
                }

                if (semester.ConfigSemester.DaysForStudy == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                int disciplinesCount = semester.Disciplines.Count();

                List<DisciplineModel> disciplines = semester.Disciplines.ToList();

                Dictionary<string, DisciplineModel> studyPlan = new Dictionary<string, DisciplineModel>();

                int daysRest = semester.ConfigSemester.DaysForStudy.Split(",").Count() - disciplinesCount;
                bool usedDaysRest = true;

                foreach (string day in semester.ConfigSemester.DaysForStudy.Split(","))
                {
                    if (daysRest > 0 && disciplinesCount != semester.Disciplines.Count() && !usedDaysRest)
                    {
                        daysRest = daysRest - 1;
                        usedDaysRest = true;
                        continue;
                    }

                    if (disciplinesCount > 0)
                        disciplinesCount = disciplinesCount - 1;
                    else
                        disciplinesCount = semester.Disciplines.Count();

                    studyPlan.Add(day, disciplines[disciplinesCount]);

                    usedDaysRest = false;
                }

                List<DayOfWeekModel> days = new List<DayOfWeekModel>();

                foreach (var item in studyPlan)
                {
                    days.Add(new DayOfWeekModel(item, semester.ConfigSemester.HourBeginStudy));
                }

                await _sudyService.Create(days);

                return StatusCode(StatusCodes.Status200OK, days);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        [ActionName(nameof(DeleteSemester))]
        [Authorize]
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
