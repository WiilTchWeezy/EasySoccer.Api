using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests.Base;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [ApiController]
    public class SoccerPitchReservationController : ApiBaseController
    {
        private SoccerPitchReservationUoW _uow;
        public SoccerPitchReservationController(SoccerPitchReservationUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [AllowAnonymous]
        [Route("api/soccerpitchreservation/get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetBaseRequest request)
        {
            try
            {
                return Ok((await _uow.SoccerPitchReservationBLL.GetAsync(DateTime.Now, 1, request.Page, request.PageSize)).Select(x => new
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

    }
}