using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using StudandoApi.Models.User;
using SudyApi.Models.Subject;
using SudyApi.Properties.Enuns;

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

        public async Task<IActionResult> GetAllSubjects(int limit = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(SubjectModel.SubjectId))
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

        public async Task<IActionResult> GetSubject (int? subjectId, string? name)
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

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
