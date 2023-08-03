﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateSchedule()
        {
            try
            {

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
