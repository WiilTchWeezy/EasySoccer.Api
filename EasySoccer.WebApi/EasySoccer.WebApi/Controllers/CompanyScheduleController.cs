using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyScheduleController : ApiBaseController
    {
        private CompanyScheduleUoW _uow;
        public CompanyScheduleController(CompanyScheduleUoW uoW) : base(uoW)
        {
            _uow = uoW;
        }

        [AllowAnonymous]
        [Route("get"), HttpGet]
        public async Task<IActionResult> GetCompanyUserInformationAsync([FromQuery] int companyId, [FromQuery]int dayOfWeek)
        {
            try
            {
                var companySchedule = await _uow.CompanyScheduleBLL.GetCompanyScheduleByDay(companyId, dayOfWeek);
                var hoursOptions = new List<string>();
                if (companySchedule != null)
                {
                    for (int i = (int)companySchedule?.StartHour; i < (int)companySchedule?.FinalHour; i++)
                    {
                        hoursOptions.Add($"{i}:00");
                        hoursOptions.Add($"{i}:30");
                    }
                    hoursOptions.Add($"{companySchedule?.FinalHour}:00");
                }
                return Ok(new
                {
                    Data = hoursOptions
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}