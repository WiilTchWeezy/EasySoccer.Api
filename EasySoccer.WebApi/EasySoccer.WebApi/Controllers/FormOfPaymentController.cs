using EasySoccer.WebApi.ApiRequests;
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
    public class FormOfPaymentController : ApiBaseController
    {
        FormOfPaymentUoW _uow;
        public FormOfPaymentController(FormOfPaymentUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] FormOfPaymentRequest request)
        {
            try
            {
                return Ok(await _uow.FormOfPaymentBLL.CreateAsync(request.Name, request.Active, new CurrentUser(HttpContext).CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] FormOfPaymentRequest request)
        {
            try
            {
                return Ok(await _uow.FormOfPaymentBLL.UpdateAsync(request.FormOfPaymentId, request.Name, request.Active, new CurrentUser(HttpContext).CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetFormOfPaymentRequest request)
        {
            try
            {
                var data = await _uow.FormOfPaymentBLL.GetAsync(request.Page, request.PageSize, new CurrentUser(HttpContext).CompanyId);
                var total = await _uow.FormOfPaymentBLL.GetTotalAsync(new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    Data = data.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Active,
                        x.CreatedDate
                    }).ToList(),
                    Total = total
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("getDropDown"), HttpGet]
        public async Task<IActionResult> GetDropDownAsync()
        {
            try
            {
                return Ok((await _uow.FormOfPaymentBLL.GetAsync(new CurrentUser(HttpContext).CompanyId)).Where(x => x.Active).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Active,
                    x.CreatedDate
                }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
