using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Controllers
{
    public class SemesterController : Controller
    {
        private readonly ISudyService _sudyService;

        public SemesterController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllSemesters))]
        [Authorize]
        public async Task<IActionResult> GetAllSemesters(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(UserModel.UserId))
        {
            try
            {
                List<UserModel> users = await _sudyService.UserRepository.GetAllUsersNoTracking(limit, ordering, attributeName);

                if (users.Count == 0)
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
