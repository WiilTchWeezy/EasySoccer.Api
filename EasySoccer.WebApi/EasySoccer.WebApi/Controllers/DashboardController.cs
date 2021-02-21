using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.BLL.Infra.Helpers;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
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
        
        [Route("reservations"), HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var currentUser = new CurrentUser(HttpContext);
                return Ok((await _uow.SoccerPitchReservationBLL.GetResumeAsync()).Select(x => new
                {
                    x.PersonId,
                    UserName = x.Person.Name,
                    UserPhone = x.Person.Phone,
                    SoccerPitchName = x.SoccerPitch.Name,
                    SelectedHour = new DateTime(x.SelectedDateStart.TimeOfDay.Ticks).ToString("hh:mm") + "-" + new DateTime(x.SelectedDateEnd.TimeOfDay.Ticks).ToString("hh:mm")
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        
        [Route("reservationschart"), HttpGet]
        public async Task<IActionResult> GetReservationsChart()
        {
            try
            {
                return Ok(await _uow.SoccerPitchReservationBLL.GetReservationChartDataAsync(DateTime.Now));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("reservationscalendar"), HttpGet]
        public async Task<IActionResult> GetReservationsCalendar([FromQuery]int month, [FromQuery]int year, [FromQuery]int? day = null)
        {
            try
            {
                var retorno = await _uow.SoccerPitchReservationBLL.GetReservationsByMonthOrDay(month, day, new CurrentUser(this.HttpContext).CompanyId, year);
                return Ok
                    (
                    retorno
                        .Select(x => new
                        {
                            startDate = x.SelectedDateStart,
                            endDate = x.SelectedDateEnd,
                            title = $"{x.SoccerPitch.Name} - {x.Person?.Name} - ( {x.SelectedDateStart.TimeOfDay.Hours:00}:{x.SelectedDateStart.TimeOfDay.Minutes:00} - {x.SelectedDateEnd.TimeOfDay.Hours:00}:{x.SelectedDateEnd.TimeOfDay.Minutes:00} ) - {EnumHelper.Instance.GetStatusEnumDescription(x.Status)}",
                            color = string.IsNullOrEmpty(x.SoccerPitch.Color) ? "#ff591f" : x.SoccerPitch?.Color,
                            id = x.Id,
                            status = x.Status,
                            statusDescription = EnumHelper.Instance.GetStatusEnumDescription(x.Status)
                        }).ToList()
                    );
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}