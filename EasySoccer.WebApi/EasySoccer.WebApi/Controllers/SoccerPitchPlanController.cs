using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.ApiRequests.Base;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoccerPitchPlanController : ApiBaseController
    {
        private SoccerPitchPlanUoW _uow;
        public SoccerPitchPlanController(SoccerPitchPlanUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [AllowAnonymous]
        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetBaseRequest request)
        {
            try
            {
                return Ok((await _uow.SoccerPitchPlanBLL.GetAsync(1, request.Page, request.PageSize)).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Value
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SoccerPitchPlanRequest request)
        {
            try
            {
                await _uow.SoccerPitchPlanBLL.CreateAsync(request.Name, request.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]SoccerPitchPlanRequest request)
        {
            try
            {
                await _uow.SoccerPitchPlanBLL.UpdateAsync(request.id, request.Name, request.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}