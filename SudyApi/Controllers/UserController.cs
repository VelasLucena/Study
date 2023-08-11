﻿using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly ISudyService _sudyService;

        public UserController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetUserList))]
        [Authorize]
        public async Task<IActionResult> GetUserList(int take = 100, Ordering ordering = Ordering.Desc, string attributeName = nameof(UserModel.UserId))
        {
            try
            {
                _sudyService.DataOptions.KeyOrder = attributeName;
                _sudyService.DataOptions.Take = take;
                _sudyService.DataOptions.Ordering = ordering;

                List<UserModel> users = await _sudyService.UserRepository.GetAllUsers();

                if (users.Count == 0)
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
                    user = await _sudyService.UserRepository.GetUserById(Convert.ToInt32(userId));
                else if (name != null)
                    user = await _sudyService.UserRepository.GetUserByNameFirst(name);
                else
                    return BadRequest();

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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

                _sudyService.DataOptions.IsTracking = true;

                UserModel newUser = new UserModel(user);

                await _sudyService.Create(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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

                _sudyService.DataOptions.IsTracking = true;

                UserModel editUser = await _sudyService.UserRepository.GetUserById(user.UserId);

                if(editUser == null) 
                    return NotFound();

                editUser.Update(user);

                await _sudyService.Update(editUser);

                return Ok(editUser);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteUser))]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                _sudyService.DataOptions.IsTracking = true;

                UserModel user = await _sudyService.UserRepository.GetUserById(userId);

                if (user == null)
                    return NotFound();

                await _sudyService.Delete(user.UserInformation);
                await _sudyService.Delete(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
