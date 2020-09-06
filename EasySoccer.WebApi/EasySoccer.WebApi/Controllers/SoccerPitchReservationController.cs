using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.BLL.Exceptions;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.ApiRequests.Base;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoccerPitchReservationController : ApiBaseController
    {
        private SoccerPitchReservationUoW _uow;
        public SoccerPitchReservationController(SoccerPitchReservationUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetSoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(new
                {
                    Data = (await _uow.SoccerPitchReservationBLL.GetAsync(
                        new CurrentUser(HttpContext).CompanyId,
                        request.Page,
                        request.PageSize,
                        request.InitialDate,
                        request.FinalDate,
                        request.SoccerPitchId,
                        request.SoccerPitchPlanId,
                        request.UserName)
                    ).Select(x => new
                    {
                        x.Id,
                        SelectedDate = x.SelectedDateStart,
                        SelectedHourStart = new { Hour = x.SelectedDateStart.TimeOfDay.Hours.ToString("00"), Minute = x.SelectedDateStart.TimeOfDay.Minutes.ToString("00") },
                        SelectedHourEnd = new { Hour = x.SelectedDateEnd.TimeOfDay.Hours.ToString("00"), Minute = x.SelectedDateEnd.TimeOfDay.Minutes.ToString("00") },
                        SoccerPitchName = x.SoccerPitch.Name,
                        UserName = x.Person != null ? x.Person?.Name : "Responsável não selecionado",
                        UserPhone = x.Person?.Phone,
                        UserId = x.Person?.UserId,
                        x.SoccerPitchId,
                        x.Status,
                        x.SoccerPitchSoccerPitchPlanId,
                        x.Interval,
                        SoccerPitchPlanId = x.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId
                    }).ToList(),
                    Total = await _uow.SoccerPitchReservationBLL.GetTotalAsync(new CurrentUser(HttpContext).CompanyId, request.InitialDate, request.FinalDate, request.SoccerPitchId, request.SoccerPitchPlanId, request.UserName)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.CreateAsync(request.SoccerPitchId, request.PersonId, request.SelectedDate, request.HourStart, request.HourEnd, request.Note, new CurrentUser(HttpContext).UserId, request.SoccerPitchPlanId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]SoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.UpdateAsync(request.Id, request.SoccerPitchId, request.PersonId, request.SelectedDate, request.HourStart, request.HourEnd, request.Note, request.SoccerPitchPlanId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("getavaliableschedules"), HttpGet]
        public async Task<IActionResult> GetSportTypeAsync(long companyId, DateTime selectedDate, int sportType)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.GetAvaliableSchedules(companyId, selectedDate, sportType));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("makeschedule"), HttpPost]
        public async Task<IActionResult> MakeScheduleAsync([FromBody]SoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.CreateAsync(request.SoccerPitchId, new MobileUser(HttpContext).UserId, request.SelectedDate, request.HourStart, request.HourEnd, request.Note, request.SoccerPitchSoccerPitchPlanId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("getuserschedules"), HttpGet]
        public async Task<IActionResult> GetUserSchedulesAsync()
        {
            try
            {
                return Ok((await _uow.SoccerPitchReservationBLL.GetUserSchedulesAsync(new MobileUser(HttpContext).UserId)).Select(x => new
                {
                    x.Id,
                    SelectedDate = x.SelectedDateStart,
                    SelectedHourStart = x.SelectedDateStart.TimeOfDay,
                    SelectedHourEnd = x.SelectedDateEnd.TimeOfDay,
                    SoccerPitchName = x.SoccerPitch.Name,
                    UserName = x.Person != null ? x.Person?.Name : "Responsável não selecionado",
                    UserPhone = x.Person?.Phone,
                    UserId = x.Person?.UserId,
                    x.SoccerPitchId,
                    x.Status,
                    x.SoccerPitchSoccerPitchPlanId,
                    CompanyName = x.SoccerPitch.Company.Name,
                    Logo = x.SoccerPitch.Company.Logo
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("getschedules"), HttpGet]
        public async Task<IActionResult> GetSchedulesResponses([FromQuery]long companyId, [FromQuery]DateTime selectedDate)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.GetSchedulesResponses(companyId, selectedDate));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}