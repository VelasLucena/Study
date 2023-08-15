using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class SubjectController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public SubjectController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetSubjectList))]
        [Authorize]
        public async Task<IActionResult> GetSubjectList(int take = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(SubjectModel.SubjectId))
        {
            try
            {
                _sudyService.DataOptions.KeyOrder = attributeName;
                _sudyService.DataOptions.Take = take;
                _sudyService.DataOptions.Ordering = ordering;

                List<SubjectModel> subjects = await _sudyService.SubjectRepository.GetAllSubjects();

                if (subjects.Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetSubject))]
        [Authorize]
        public async Task<IActionResult> GetSubject(int? subjectId, string? name)
        {
            try
            {
                SubjectModel subject = new SubjectModel();

                if (subjectId.HasValue)
                    subject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(Convert.ToInt32(subjectId));
                else if (!string.IsNullOrEmpty(name))
                    subject = await _sudyService.SubjectRepository.GetSubjectByNameFirst(name);
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                if(subject == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, subject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateSubject))]
        [Authorize]
        public async Task<IActionResult> CreateSubject(RegisterSubjectViewModel subject)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                _sudyService.DataOptions.IsTracking = true;

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineById(subject.DisciplineId);

                if(discipline == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                SubjectModel newSubject = new SubjectModel(subject);

                await _sudyService.Create(newSubject);

                return StatusCode(StatusCodes.Status200OK, newSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ActionName(nameof(EditSubject))]
        [Authorize]
        public async Task<IActionResult> EditSubject(EditSubjectViewModel subject)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                _sudyService.DataOptions.IsTracking = true;

                SubjectModel editSubject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subject.SubjectId);

                if(editSubject == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                editSubject.Update(subject);

                await _sudyService.Update(editSubject);       

                return StatusCode(StatusCodes.Status200OK, editSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteSubject))]
        [Authorize]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                SubjectModel subject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subjectId);

                if(subject == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                await _sudyService.Delete(subject);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
