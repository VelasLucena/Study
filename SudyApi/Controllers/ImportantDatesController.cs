using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class ImportantDatesController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ImportantDatesController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(CreateImportantDate))]
        [Authorize]
        public async Task<IActionResult> CreateImportantDate(RegisterImportanteDateViewModel importanteDate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                ImportantDateModel newImportantDate = new ImportantDateModel(importanteDate);

                await _sudyService.Create(newImportantDate);

                return Ok(newImportantDate);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
