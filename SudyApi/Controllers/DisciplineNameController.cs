using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Utility;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    [Authorize]
    public class DisciplineNameController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public DisciplineNameController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet(nameof(List))]
        public async Task<IActionResult> List(string name)
        {
            try
            {
                List<DisciplineNameModel> disciplines = await _sudyService.DisciplineNameRepository.GetListDisciplineNameByName(name);

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
        public async Task<IActionResult> CreateDisciplineName(string disciplineName)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                DisciplineNameModel newDiscipline = await _sudyService.DisciplineNameRepository.GetDisciplineNameByName(disciplineName);

                if (newDiscipline == null)
                {
                    if (!InappropriateWords.WordIsInappropriate(disciplineName))
                    {
                        newDiscipline = new DisciplineNameModel(disciplineName);
                        await _sudyService.Create(newDiscipline);
                    }
                    else
                        return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest);

                return StatusCode(StatusCodes.Status200OK, newDiscipline);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
