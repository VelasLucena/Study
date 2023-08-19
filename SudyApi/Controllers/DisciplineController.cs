using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    [Authorize]
    public class DisciplineController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public DisciplineController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscipline(int semesterId, int? discplineId, int? disciplineNameId)
        {
            try
            {
                DisciplineModel discipline = new DisciplineModel();

                if (discplineId != null)
                    discipline = await _sudyService.DisciplineRepository.GetDisciplineById(discplineId.Value);
                else if (disciplineNameId != null)
                    discipline = await _sudyService.DisciplineRepository.GetDisciplineByName(disciplineNameId.Value, semesterId);
                else
                    return StatusCode(StatusCodes.Status400BadRequest);
                
                if (discipline == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, discipline);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet(nameof(List))]
        public async Task<IActionResult> List(string? name, int semesterId, int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
        {
            try
            {
                List<DisciplineModel> disciplines = new List<DisciplineModel>();

                if (name != null)
                {
                    List<DisciplineNameModel> disciplinesnames = await _sudyService.DisciplineNameRepository.GetListDisciplineNameByName(name);

                    _sudyService.DataOptions.KeyOrder = attributeName;
                    _sudyService.DataOptions.Take = take;
                    _sudyService.DataOptions.Ordering = ordering;

                    if (disciplinesnames.Count == 0)
                        return StatusCode(StatusCodes.Status404NotFound);

                    foreach (DisciplineNameModel item in disciplinesnames)
                    {
                        DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByName(item.DisciplineNameId, semesterId);

                        if (discipline != null)
                            disciplines.Add(discipline);
                    }
                }
                else
                    disciplines = await _sudyService.DisciplineRepository.GetDisciplinesBySemesterId(semesterId);                 

                if (disciplines.Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, disciplines);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(RegisterDisciplineViewModel discipline)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                _sudyService.DataOptions.IsTracking = true;

                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(discipline.SemesterId);

                if (semester == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                DisciplineNameModel disciplineName = await _sudyService.DisciplineNameRepository.GetDisciplineNameById(discipline.DisciplineNameId);

                DisciplineModel newDiscipline = new DisciplineModel(semester, disciplineName, discipline);

                await _sudyService.Create(newDiscipline);

                return StatusCode(StatusCodes.Status200OK, newDiscipline);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditDiscipline(EditDisciplineViewModel discipline)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(Convert.ToInt32(discipline.SemesterId));

                if (semester == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                DisciplineNameModel disciplineName = await _sudyService.DisciplineNameRepository.GetDisciplineNameById(Convert.ToInt32(discipline.DisciplineNameId));

                DisciplineModel editDiscipline = await _sudyService.DisciplineRepository.GetDisciplineById(discipline.DisciplineId);

                editDiscipline.Update(semester, disciplineName, discipline);

                await _sudyService.Update(editDiscipline);

                return StatusCode(StatusCodes.Status200OK, editDiscipline);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineById(disciplineId);

                if (discipline == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                List<SubjectModel> subjects = await _sudyService.SubjectRepository.GetSubjectByDisciplineId(disciplineId);

                if (subjects.Count > 0)
                    await _sudyService.Delete(subjects);

                await _sudyService.Delete(discipline);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
