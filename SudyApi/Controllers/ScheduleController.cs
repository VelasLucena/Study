using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ISudyService _sudyService;

        public ScheduleController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(CreateSchedule))]
        public async Task<IActionResult> CreateSchedule(RegisterUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState });

                UserModel newUser = new UserModel(user);

                await _sudyService.Create(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
