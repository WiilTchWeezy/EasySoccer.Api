
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
    public class PaymentController : ApiBaseController
    {
        private PaymentUoW _uow;
        public PaymentController(PaymentUoW uoW) : base(uoW)
        {
            _uow = uoW;
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PaymentRequest request)
        {
            try
            {
                return Ok(await _uow.PaymentBLL.CreateAsync(request.Value, request.SoccerPitchReservationId, request.PersonCompanyId, request.Note, request.FormOfPaymentId, new CurrentUser(HttpContext).UserId, new CurrentUser(HttpContext).CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] PaymentRequest request)
        {
            try
            {
                return Ok(await _uow.PaymentBLL.UpdateAsync(request.PaymentId, request.Value, request.PersonCompanyId, request.Note, request.FormOfPaymentId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] Guid soccerPitchReservationId)
        {
            try
            {
                var data = await _uow.PaymentBLL.GetAsync(soccerPitchReservationId);
                return Ok(new
                {
                    Total = data.Sum(x => x.Value),
                    Data = data.Select(x => new
                    {
                        x.Note,
                        x.Id,
                        x.PersonCompanyId,
                        x.SoccerPitchReservationId,
                        x.Value,
                        PersonCompanyName = x.PersonCompany != null ? x.PersonCompany.Name : "Sem Resp.",
                        CreatedDate = x.CreatedDate.AddHours(-3),//TODO - Field on Company
                        FormOfPaymentName = x.FormOfPayment?.Name
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
