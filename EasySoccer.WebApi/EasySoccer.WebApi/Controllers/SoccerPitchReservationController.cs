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
        [EnableCors]
        public async Task<IActionResult> GetAsync([FromQuery]GetBaseRequest request)
        {
            return Ok((await _uow.SoccerPitchReservationBLL.GetAsync(DateTime.Now, 1, request.Page, request.PageSize)).Select(x => new
            {
                x.Id,
                x.SelectedDate,
                x.SelectedHourEnd,
                x.SelectedHourStart,
                SoccerPitchName = x.SoccerPitch.Name,
                UserName = x.User.Name,
                x.UserId,
                x.SoccerPitchId
            }).ToList());
        }

    }
}