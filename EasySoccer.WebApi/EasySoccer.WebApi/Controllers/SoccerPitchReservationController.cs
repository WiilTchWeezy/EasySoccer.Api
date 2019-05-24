using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.ApiRequests.Base;
using EasySoccer.WebApi.Controllers.Base;
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

        [AllowAnonymous]
        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetBaseRequest request)
        {
            try
            {
                return Ok((await _uow.SoccerPitchReservationBLL.GetAsync(1, request.Page, request.PageSize)).Select(x => new
                {
                    x.Id,
                    x.SelectedDate,
                    SelectedHourStart = x.SelectedHourStart.ToString("hh':'mm"),
                    SelectedHourEnd = x.SelectedHourEnd.ToString("hh':'mm"),
                    SoccerPitchName = x.SoccerPitch.Name,
                    UserName = x.User.Name,
                    x.UserId,
                    x.SoccerPitchId,
                    x.Status
                }).ToList());
            }catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.CreateAsync(request.SoccerPitchId, request.UserId, request.SelectedDate, request.HourStart, request.HourFinish, request.Note, request.CompanyUserId));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]SoccerPitchReservationRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.UpdateAsync(request.Id, request.SoccerPitchId, request.UserId, request.SelectedDate, request.HourStart, request.HourFinish, request.Note));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

    }
}