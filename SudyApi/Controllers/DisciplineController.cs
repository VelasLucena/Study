using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Utility;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class DisciplineController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public DisciplineController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetNameDisciplineList))]
        [Authorize]
        public async Task<IActionResult> GetNameDisciplineList(string name)
        {
            try
            {
                List<DisciplineNameModel> disciplines = await _sudyService.DisciplineNameRepository.GetDisciplineNameByNameNoTracking(name);

                if (disciplines.Count == 0)
                    return NotFound();

                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetDiscipline))]
        [Authorize]
        public async Task<IActionResult> GetDiscipline(int? semesterId, int? discplineId, int? disciplineNameId)
        {
            try
            {
                DisciplineModel discipline = new DisciplineModel();

                if (discplineId != null)
                    discipline = await _sudyService.DisciplineRepository.GetDisciplineByIdNoTracking(discplineId.Value);
                else if (disciplineNameId != null && semesterId != null)
                    discipline = await _sudyService.DisciplineRepository.GetDisciplineByNameNoTracking(discplineId.Value, semesterId.Value);
                
                if (discipline != null)
                    return NotFound();

                return Ok(discipline);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetDisciplinesList))]
        [Authorize]
        public async Task<IActionResult> GetDisciplinesList(string? name, int? semesterId, int? discplineId, int? disciplineNameId)
        {
            try
            {
                List<DisciplineModel> disciplines = new List<DisciplineModel>();

                if (discplineId != null && semesterId != null)
                {
                    List<DisciplineNameModel> disciplinesnames = await _sudyService.DisciplineNameRepository.GetDisciplineNameByNameNoTracking(name);

                    if (disciplinesnames.Count == 0)
                        return NotFound();

                    foreach (DisciplineNameModel item in disciplinesnames)
                    {
                        DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByNameNoTracking(item.DisciplineNameId, semesterId.Value);

                        if (discipline != null)
                            disciplines.Add(discipline);
                    }
                }
                else if (semesterId != null)
                    disciplines = await _sudyService.DisciplineRepository.GetDisciplinesBySemesterIdNoTracking(semesterId.Value);                 
                else
                    return BadRequest();

                if (disciplines.Count == 0)
                    return NotFound();

                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
                        await _sudyService.Create(disciplineName);
                    }
                }

                DisciplineModel newDiscipline = new DisciplineModel(semester, disciplineName, discipline);

                await _sudyService.Create(newDiscipline);

                return Ok(newDiscipline);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
                        await _sudyService.Create(disciplineName);
                    }
                }

                DisciplineModel editDiscipline = await _sudyService.DisciplineRepository.GetDisciplineById(discipline.DisciplineId);

                editDiscipline.Update(semester, disciplineName, discipline);

                await _sudyService.Update(editDiscipline);

                return Ok(editDiscipline);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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

                if (subjects.Count > 0)
                    await _sudyService.Delete(subjects);

                await _sudyService.Delete(discipline);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
