using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Utility;
using SudyApi.ViewModels;

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
                List<DisciplineModel> disciplines = await _sudyService.DisciplineRepository.GetAllDisciplinesNoTracking(semesterId);

                if (disciplines.Count == 0)
                    return NotFound();

                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [ActionName(nameof(GetNameDisciplineList))]
        [Authorize]
        public async Task<IActionResult> GetNameDisciplineList(string name)
        {
            try
            {
                List<string> disciplines = await _sudyService.DisciplineNameRepository.GetDisciplineNameByNameNoTracking(name);

                if (disciplines.Count == 0)
                    return NotFound();

                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateDiscipline))]
        [Authorize]
        public async Task<IActionResult> CreateDiscipline(RegisterDisciplineViewModel discipline)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(discipline.SemesterId);

                if (semester == null)
                    return NotFound();

                DisciplineNameModel disciplineName = await _sudyService.DisciplineNameRepository.GetDisciplineNameById(discipline.DisciplineNameId);

                if(disciplineName == null)
                {
                    if (InappropriateWords.WordIsInappropriate(discipline.DisciplineName))
                    {
                        disciplineName.Update(discipline.DisciplineName);
                        _sudyService.Create<DisciplineNameModel>(disciplineName);
                    }
                }

                DisciplineModel newDiscipline = new DisciplineModel();

                await _sudyService.Create(newDiscipline);

                return Ok(newDiscipline);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [ActionName(nameof(EditDiscipline))]
        [Authorize]
        public async Task<IActionResult> EditDiscipline(EditDisciplineViewModel discipline)
        {
            try
            {
                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(Convert.ToInt32(discipline.SemesterId));

                if (semester == null)
                    return NotFound();

                DisciplineNameModel disciplineName = await _sudyService.DisciplineNameRepository.GetDisciplineNameById(Convert.ToInt32(discipline.DisciplineNameId));

                if (disciplineName == null)
                {
                    if (InappropriateWords.WordIsInappropriate(discipline.DisciplineName))
                    {
                        disciplineName.Update(discipline.DisciplineName);
                        _sudyService.Create<DisciplineNameModel>(disciplineName);
                    }
                }

                DisciplineModel editDiscipline = await _sudyService.DisciplineRepository.GetDisciplineById(discipline.DisciplineId);

                editDiscipline.Update(semester, disciplineName);

                await _sudyService.Update(editDiscipline);

                return Ok(editDiscipline);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteDiscipline))]
        [Authorize]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        {
            try
            {
                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineById(disciplineId);

                if (discipline == null)
                    return NotFound();

                List<SubjectModel> subjects = await _sudyService.SubjectRepository.GetSubjectByDisciplineId(disciplineId);

                if (subjects.Count == 0)
                    return NotFound();

                await _sudyService.Delete(discipline);
                await _sudyService.DeleteMany(subjects);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
