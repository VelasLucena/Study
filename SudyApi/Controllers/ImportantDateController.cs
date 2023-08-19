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
    [Route("api/{controller}")]
    [Authorize]
    public class ImportantDateController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public ImportantDateController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet(nameof(List))]
        public async Task<IActionResult> List(DateOnly? date, int? scheduleId, int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
        {
            try
            {
                _sudyService.DataOptions.KeyOrder = attributeName;
                _sudyService.DataOptions.Take = take;
                _sudyService.DataOptions.Ordering = ordering;

                List<ImportantDateModel> importantDates = new List<ImportantDateModel>();

                if (date != null)
                    importantDates = await _sudyService.ImportanteDateRepository.GetImportantDateByDate(date.Value);
                else if (scheduleId != null)
                    importantDates = await _sudyService.ImportanteDateRepository.GetAllImportantDateBySemesterId(scheduleId.Value);
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState });

                if (importantDates == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, importantDates);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImportantDate(int importantDateId)
        {
            try
            {
                ImportantDateModel importantDate = await _sudyService.ImportanteDateRepository.GetImportantDateById(importantDateId);

                if (importantDate == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, importantDate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }        

        [HttpPost]
        public async Task<IActionResult> CreateImportantDate(RegisterImportanteDateViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);

                _sudyService.DataOptions.IsTracking = true;

                SemesterModel semester = await _sudyService.SemesterRepository.GetSemesterById(viewModel.SemesterId);

                if (semester == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                ImportantDateModel newImportantDate = viewModel;

                await _sudyService.Create(newImportantDate);

                return StatusCode(StatusCodes.Status200OK, newImportantDate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }   

        [HttpPut]
        public async Task<IActionResult> EditImportantDate(EditImportantDateViewModel importanteDate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                _sudyService.DataOptions.IsTracking = true;

                ImportantDateModel importanteDateOld = await _sudyService.ImportanteDateRepository.GetImportantDateById(importanteDate.ImportantDateId);

                if (importanteDateOld == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                importanteDateOld.Update(importanteDate);

                await _sudyService.Update(importanteDateOld);

                return StatusCode(StatusCodes.Status200OK, importanteDateOld);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImportantDate(int importantDateId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                ImportantDateModel importantDate = await _sudyService.ImportanteDateRepository.GetImportantDateById(importantDateId);

                if (importantDate == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                await _sudyService.Delete(importantDate);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
