using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;

namespace SudyApi.Controllers
{
    public class DisciplineController : Controller
    {
        private readonly ISudyService _sudyService;

        public DisciplineController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllDisciplines))]
        [Authorize]
        public async Task<IActionResult> GetAllDisciplines(int semesterId)
        {
            try
            {
                List<DisciplineModel> semesters = await _sudyService.DisciplineRepository.GetAllDisciplinesNoTracking(semesterId);

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
        [ActionName(nameof(GetDiscipline))]
        [Authorize]
        public async Task<IActionResult> GetDiscipline(string? name)
        {
            try
            {
                List<SemesterModel> semester = await _sudyService.DisciplineRepository.

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
