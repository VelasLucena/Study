using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Resources;
using SudyApi.Security;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("{controller}/{action}")]
    public class AuthorizationController : Controller
    {
        private readonly ISudyService _sudyService;

        public AuthorizationController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost]
        [ActionName(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                UserModel user = await _sudyService.UserRepository.GetUserByEmail(userLogin.Email);

                if (user == null)
                    return NotFound();

                if (!EncryptPassord.VerifyHashedPassword(user.PasswordHash, userLogin.Password))
                    return BadRequest();

                user.Token = Token.GenerateToken(user);
                user.UpdateDate = DateTime.UtcNow;
                user.UpdateUser = user.UserId;

                await _sudyService.Update(user);

                return Ok(new { user.Token });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ActionName(nameof(Logout))]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                UserModel user = await _sudyService.UserRepository.GetUserById(UserLogged.UserId);

                if (user == null)
                    return NotFound();

                user.Token = null;
                user.UpdateDate = DateTime.UtcNow;
                user.UpdateUser = UserLogged.UserId;

                await _sudyService.Update(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
