using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ScheduleController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(CreateScheduleSemester))]
        public async Task<IActionResult> CreateScheduleSemester(int semesterId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

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
                        return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState });
                }

                if (semester.ConfigSemester.DaysForStudy == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState });

                int disciplinesCount = semester.Disciplines.Count();

                List<DisciplineModel> disciplines = semester.Disciplines.ToList();

                Dictionary<string, DisciplineModel> studyPlan = new Dictionary<string, DisciplineModel>();

                int daysRest = semester.ConfigSemester.DaysForStudy.Split(",").Count() - disciplinesCount;
                bool usedDaysRest = true;
                string daysRestString = semester.ConfigSemester.DaysForStudy;

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

                    daysRestString.Replace(day, "");

                    studyPlan.Add(day, disciplines[disciplinesCount]);

                    usedDaysRest = false;
                }

                List<DayOfWeekModel> days = new List<DayOfWeekModel>();

                foreach (var item in studyPlan)
                {
                    days.Add(new DayOfWeekModel(item, semester.ConfigSemester.HourBeginStudy));
                }

                string[] daysRestStringArray = daysRestString.Split(",");

                Dictionary<string, DisciplineModel> planOptional = new Dictionary<string, DisciplineModel>();

                //foreach (var item in semester.Disciplines)
                //{
                //    if (DayOfWeekModel.DisciplineHasPossible())
                //    {
                //        if (daysRestString != string.Empty)
                //        {
                //            planOptional.Add();
                //            days.Add(new DayOfWeekModel(item, semester.ConfigSemester.HourBeginStudy));
                //        }
                //    }
                //}

                await _sudyService.Create(days);

                return StatusCode(StatusCodes.Status200OK, days);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
