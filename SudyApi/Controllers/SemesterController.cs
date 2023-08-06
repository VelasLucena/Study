﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Code;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;

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
        public async Task<IActionResult> GetSemestersList(int userId)
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
                return Problem(ex.Message);
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
                return Problem(ex.Message);
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

                if (course == null)
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
                return Problem(ex.Message);
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
                        return BadRequest();
                }

                if (semester.ConfigSemester.DaysForStudy != null)
                    daysForStudy = semester.ConfigSemester.DaysForStudy.Split(",").ToList();

                if()

                List<string> daysForStudy = new List<string>();

                List<DayOfWeekModel> days = new List<DayOfWeekModel>();

                int DayDisciplineCount = 1;
                int DaySubjectCount = 1;
                int disciplinesCount = semester.Disciplines.Count;

                foreach (string day in daysForStudy)
                {
                    DayOfWeek today = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);

                    if (DayDisciplineCount > semester.Disciplines.Count)
                        DayDisciplineCount = 1;

                    DisciplineModel disciplineToday = semester.Disciplines.ToList()[DayDisciplineCount];

                    int totalModulesCount = 0;

                    foreach(var subject in disciplineToday.Subjects)
                    {
                        foreach(var chapter in subject.Chapters)
                        {
                            totalModulesCount = chapter.ModulesCount + totalModulesCount;
                        }
                    }

                    int totalDaysToStudy = 0;

                    if (semester.ConfigSemester.HoursForStudy != null)
                        totalDaysToStudy = totalModulesCount / semester.ConfigSemester.HoursForStudy.Value;
                    else
                        return Problem();

                    await _sudyService.Create(new DayOfWeekModel(disciplineToday, today, semester.ConfigSemester.HourBeginStudy, totalModulesCount, totalDaysToStudy));
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
                return Problem(ex.Message);
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

                    if (subjects.Count > 0)
                    {
                        foreach (SubjectModel item in subjects)
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

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
