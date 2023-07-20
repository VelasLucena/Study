using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;

namespace SudyApi.Controllers
{
    public class CollegeSubjectController : Controller
    {
        private readonly ISudyService _sudyService;

        public CollegeSubjectController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllCollegeSubjects))]
        [Authorize]
        public async Task<IActionResult> GetAllCollegeSubjects(int semesterId)
        {
            try
            {
                List<CollegeSubjectModel> semesters = await _sudyService.collegeSubjectRepository.GetAllCollegeSubjectsNoTracking(semesterId);

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
        [ActionName(nameof(GetCollegeSubject))]
        [Authorize]
        public async Task<IActionResult> GetCollegeSubject(string? name)
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
    }
}
