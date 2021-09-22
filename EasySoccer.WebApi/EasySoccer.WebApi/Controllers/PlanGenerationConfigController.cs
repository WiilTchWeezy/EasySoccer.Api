using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.ApiRequests.Base;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanGenerationConfigController : ApiBaseController
    {
        private PlanGenerationConfigUoW _uow;
        public PlanGenerationConfigController(PlanGenerationConfigUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetBaseRequest request)
        {
            try
            {
                var entities = await _uow.PlanGenerationConfigBLL.GetAsync(new CurrentUser(HttpContext).CompanyId, request.Page, request.PageSize);
                var total = await _uow.PlanGenerationConfigBLL.GetTotalAsync(new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    Data = entities.Select(x => new
                    {
                        x.CompanyId,
                        x.Id,
                        x.IntervalBetweenReservations,
                        x.LimitQuantity,
                        x.LimitType,
                        x.Name
                    }).ToArray(),
                    Total = total
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PlanGenerationConfigRequest request)
        {
            try
            {
                await _uow.PlanGenerationConfigBLL.CreateAsync(request.Name, request.IntervalBetweenReservations, request.LimitType, request.LimitQuantity, new CurrentUser(HttpContext).CompanyId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] PlanGenerationConfigRequest request)
        {
            try
            {
                await _uow.PlanGenerationConfigBLL.UpdateAsync(request.Id, request.Name, request.IntervalBetweenReservations, request.LimitType, request.LimitQuantity);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
