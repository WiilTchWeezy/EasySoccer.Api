using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.ApiRequests.Base;
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
    public class SoccerPitchController : ApiBaseController
    {
        private SoccerPitchUoW _uow;
        public SoccerPitchController(SoccerPitchUoW uow) : base(uow)
        {
            _uow = uow;
        }

        
        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetSoccerPitchRequest request)
        {
            try
            {
                return Ok((await _uow.SoccerPitchBLL.GetAsync(request.Page, request.PageSize, request.CompanyId)).Select(x => new
                {
                    x.Id,
                    x.Active,
                    x.Description,
                    x.HasRoof,
                    x.Name,
                    x.NumberOfPlayers,
                    x.SoccerPitchSoccerPitchPlans,
                    x.SportTypeId,
                    SportTypeName = x.SportType.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        
        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SoccerPitchRequest request)
        {
            try
            {

                var plansId = request.SoccerPitchSoccerPitchPlans?.Select(x => x.SoccerPitchPlanId).ToArray();
                return Ok(await _uow.SoccerPitchBLL.CreateAsync(request.Name, request.Description, request.HasRoof, request.NumberOfPlayers, new CurrentUser(HttpContext).CompanyId, request.Active, plansId, request.SportTypeId));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        
        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]SoccerPitchRequest request)
        {
            try
            {
                var plansId = request.SoccerPitchSoccerPitchPlans?.Select(x => x.SoccerPitchPlanId).ToArray();
                return Ok(await _uow.SoccerPitchBLL.UpdateAsync(request.Id, request.Name, request.Description, request.HasRoof, request.NumberOfPlayers, new CurrentUser(HttpContext).CompanyId, request.Active, plansId, request.SportTypeId));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("getsporttypes"), HttpGet]
        public async Task<IActionResult> GetSportTypeAsync()
        {
            try
            {
                return Ok((await _uow.SoccerPitchBLL.GetSportTypeAsync()).Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}