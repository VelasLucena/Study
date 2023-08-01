using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
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

        [HttpGet]
        [ActionName(nameof(GetAllImportantDate))]
        [Authorize]
        public async Task<IActionResult> GetAllImportantDate(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(UserModel.UserId))
        {
            try
            {
                List<ImportantDateModel> importantDates = await _sudyService.ImportanteDateRepository.GetAllImportantDateById

                if (users.Count == 0)
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetImportantDate))]
        [Authorize]
        public async Task<IActionResult> GetImportantDate(int? importantDateId, DateOnly? date)
        {
            try
            {
                ImportantDateModel importantDate = new ImportantDateModel();

                if (importantDateId != null)
                    importantDate = await _sudyService.ImportanteDateRepository.GetImportantDateById(importantDateId.Value);
                else if (date != null)
                    importantDate = await _sudyService.ImportanteDateRepository.GetImportantDateByDate(date.Value);
                else
                    return BadRequest();

                if (importantDate == null)
                    return NotFound();

                return Ok(importantDate);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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

        [HttpPut]
        [ActionName(nameof(EditImportantDate))]
        [Authorize]
        public async Task<IActionResult> EditImportantDate(EditImportantDateViewModel importanteDate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                ImportantDateModel importanteDateOld = await _sudyService.ImportanteDateRepository.GetImportantDateById(importanteDate.ImportantDateId);

                if (importanteDateOld == null)
                    return NotFound();

                importanteDateOld.Update(importanteDate);

                await _sudyService.Update(importanteDateOld);

                return Ok(importanteDateOld);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
