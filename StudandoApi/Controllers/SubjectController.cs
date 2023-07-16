using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using StudandoApi.Models.User;
using SudyApi.Models.Subject;
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

        [HttpGet]
        [ActionName(nameof(GetSubject))]
        [Authorize]
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

        [HttpPost]
        [ActionName(nameof(CreateSubject))]
        public async Task<IActionResult> CreateSubject(RegisterChapterViewModel chapter)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                SubjectModel newUser = new UserModel(user);

                await _sudyService.Create(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [ActionName(nameof(EditSubject))]
        [Authorize]
        public async Task<IActionResult> EditSubject(EditUserViewModel userEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();


                UserModel userOld = await _sudyService.UserRepository.GetUserByIdNoTracking(userEdit.UserId);

                if (userOld == null)
                    return NotFound();

                UserModel userNew = new UserModel(userEdit);

                await _sudyService.Update(userNew);

                return Ok(userNew);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteSubject))]
        [Authorize]
        public async Task<IActionResult> DeleteSubject(int userId)
        {
            try
            {
                UserModel user = await _sudyService.UserRepository.GetUserById(userId);

                if (user == null)
                    return NotFound();

                await _sudyService.Delete(user.UserInformation);
                await _sudyService.Delete(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
