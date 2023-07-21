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
    public class SubjectController : Controller
    {
        private readonly ISudyService _sudyService;

        public SubjectController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllSubjects))]
        [Authorize]
        public async Task<IActionResult> GetAllSubjects(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(SubjectModel.SubjectId))
        {
            try
            {
                List<SubjectModel> subjects = await _sudyService.SubjectRepository.GetAllSubjectsNoTracking(limit, ordering, attributeName);

                if (subjects.Count == 0)
                    return NotFound();

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
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
                    subject = await _sudyService.SubjectRepository.GetSubjectBySubjectIdNoTracking(Convert.ToInt32(subjectId));
                else if (string.IsNullOrEmpty(name))
                    subject = await _sudyService.SubjectRepository.GetSubjectByNameFirstNoTracking(name);
                else
                    return BadRequest();

                if(subject == null)
                    return NotFound();

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateSubject))]
        public async Task<IActionResult> CreateSubject(RegisterSubjectViewModel subject)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                DisciplineModel discipline = await _sudyService.DisciplineRepository.GetDisciplineByIdNoTracking(subject.DisciplineId);

                if(discipline == null)
                    return NotFound();

                SubjectModel newSubject = new SubjectModel(subject);

                await _sudyService.Create(newSubject);

                return Ok(newSubject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
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
                    return BadRequest();           

                SubjectModel editSubject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subject.SubjectId);

                if(editSubject == null)
                    return NotFound();

                editSubject.Update(subject);

                await _sudyService.Update(editSubject);       

                return Ok(editSubject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteSubject))]
        [Authorize]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            try
            {
                SubjectModel subject = await _sudyService.SubjectRepository.GetSubjectBySubjectId(subjectId);

                if(subject == null)
                    return NotFound();

                await _sudyService.Delete(subject);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
