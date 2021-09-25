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
    public class SoccerPitchPlanController : ApiBaseController
    {
        private SoccerPitchPlanUoW _uow;
        public SoccerPitchPlanController(SoccerPitchPlanUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetBaseRequest request)
        {
            try
            {
                return Ok(new
                {
                    Data = (await _uow.SoccerPitchPlanBLL.GetAsync(new CurrentUser(HttpContext).CompanyId, request.Page, request.PageSize)).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Value,
                        x.Description,
                        x.IdPlanGenerationConfig,
                        x.ShowToUser
                    }).ToList(),
                    Total = await _uow.SoccerPitchPlanBLL.GetTotalAsync(new CurrentUser(HttpContext).CompanyId)
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [AllowAnonymous]
        [Route("getbysoccerpitch"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] long soccerPitchId)
        {
            try
            {
                return Ok((await _uow.SoccerPitchPlanBLL.GetAsync(soccerPitchId)).Select(x => new
                {
                    x.SoccerPitchPlan.Id,
                    x.SoccerPitchPlan.Name,
                    x.SoccerPitchPlan.Value,
                    x.SoccerPitchPlan.Description,
                    x.IsDefault,
                    x.SoccerPitchPlan.IdPlanGenerationConfig,
                    x.SoccerPitchPlan.ShowToUser
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SoccerPitchPlanRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchPlanBLL.CreateAsync(request.Name, request.Value, new CurrentUser(HttpContext).CompanyId, request.Description, request.IdPlanGenerationConfig, request.ShowToUser));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] SoccerPitchPlanRequest request)
        {
            try
            {
                return Ok(await _uow.SoccerPitchPlanBLL.UpdateAsync(request.id, request.Name, request.Value, request.Description, request.IdPlanGenerationConfig, request.ShowToUser));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}