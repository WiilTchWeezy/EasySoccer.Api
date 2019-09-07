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
    public class DashboardController : ApiBaseController
    {
        private DashboardUoW _uow;
        public DashboardController(DashboardUoW uow) : base(uow)
        {
            _uow = uow;
        }
        
        [Route("reservations")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                return Ok((await _uow.SoccerPitchReservationBLL.GetResumeAsync()).Select(x => new
                {
                    x.UserId,
                    UserName = x.User.Name,
                    UserPhone = x.User.Phone,
                    SoccerPitchName = x.SoccerPitch.Name,
                    SelectedHour = new DateTime(x.SelectedHourStart.Ticks).ToString("hh:mm") + "-" + new DateTime(x.SelectedHourEnd.Ticks).ToString("hh:mm")
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}