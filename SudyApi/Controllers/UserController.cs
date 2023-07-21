using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using StudandoApi.Properties.Enuns;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.ViewModels;

namespace StudandoApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class UserController : Controller
    {
        private readonly ISudyService _sudyService;

        public UserController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetAllUsers))]
        [Authorize]
        public async Task<IActionResult> GetAllUsers(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(UserModel.UserId))
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

        [HttpGet]
        [ActionName(nameof(GetUser))]
        [Authorize]
        public async Task<IActionResult> GetUser(int? userId, string? name)
        {
            try
            {
                UserModel user = new UserModel();

                if (userId != null)
                    user = await _sudyService.UserRepository.GetUserByIdNoTracking(Convert.ToInt32(userId));
                else if (name != null)
                    user = await _sudyService.UserRepository.GetUserByNameFirstNoTracking(name);
                else
                    return BadRequest();

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = ModelState } );

                UserModel newUser = new UserModel(user);

                await _sudyService.Create(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [ActionName(nameof(EditUser))]
        [Authorize]
        public async Task<IActionResult> EditUser(EditUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                UserModel editUser = await _sudyService.UserRepository.GetUserByIdNoTracking(user.UserId);

                if(editUser == null) 
                    return NotFound();

                editUser.Update(user);

                await _sudyService.Update(editUser);

                return Ok(editUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteUser))]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
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
