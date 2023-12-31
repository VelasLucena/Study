﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Resources;
using SudyApi.Security;
using SudyApi.ViewModels;

namespace SudyApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public AuthorizationController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _sudyService.DataOptions.IsTracking = true;

                UserModel user = await _sudyService.UserRepository.GetUserByEmail(loginUser.Email);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                if (!EncryptPassord.VerifyHashedPassword(user.PasswordHash, loginUser.Password))
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState } );

                user.Login();

                await _sudyService.Update(user);

                return StatusCode(StatusCodes.Status200OK, new { user.Token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost(nameof(Logout))]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                UserModel user = await _sudyService.UserRepository.GetUserById(UserLogged.UserId);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                user.Logout();

                await _sudyService.Update(user);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost(nameof(FirstAcess))]
        public async Task<IActionResult> FirstAcess(RegisterUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Error = ModelState });

                _sudyService.DataOptions.IsTracking = true;

                UserModel newUser = user;

                newUser.Login();

                await _sudyService.Create(newUser);

                return StatusCode(StatusCodes.Status200OK, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
