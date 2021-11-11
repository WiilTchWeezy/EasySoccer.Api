
using EasySoccer.BLL.Infra.Helpers;
using EasySoccer.Entities.Enum;
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
                var payment = await _uow.PaymentBLL.CreateAsync(request.Value, request.SoccerPitchReservationId, request.PersonCompanyId, request.Note, request.FormOfPaymentId, new CurrentUser(HttpContext).UserId, new CurrentUser(HttpContext).CompanyId);
                return Ok(new { Id = payment.Id });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("cancel"), HttpPost]
        public async Task<IActionResult> PostAsync([FromQuery] long idPayment)
        {
            try
            {
                return Ok(await _uow.PaymentBLL.CancelAsync(idPayment, new CurrentUser(HttpContext).CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] Guid soccerPitchReservationId, [FromQuery] int? status = null)
        {
            try
            {
                PaymentStatusEnum? paymentStatus = null;
                if (status.HasValue && Enum.IsDefined(typeof(PaymentStatusEnum), status.Value))
                    paymentStatus = (PaymentStatusEnum)status;
                var data = await _uow.PaymentBLL.GetAsync(soccerPitchReservationId, paymentStatus);
                var totalApproved = await _uow.PaymentBLL.GetTotalValueAsync(soccerPitchReservationId, PaymentStatusEnum.Created);
                var totalReservation = await _uow.SoccerPitchReservationBLL.GetReservationValueAsync(soccerPitchReservationId);
                return Ok(new
                {
                    Total = totalApproved,
                    TotalReservation = totalReservation,
                    Data = data.Select(x => new
                    {
                        x.Note,
                        x.Id,
                        x.PersonCompanyId,
                        x.SoccerPitchReservationId,
                        x.Value,
                        PersonCompanyName = x.PersonCompany != null ? x.PersonCompany.Name : "Sem Resp.",
                        CreatedDate = x.CreatedDate.AddHours(-3),//TODO - Field on Company
                        FormOfPaymentName = x.FormOfPayment?.Name,
                        StatusDescription = EnumHelper.Instance.GetEnumDescription(x.Status),
                        x.Status
                    }).OrderByDescending(x => x.CreatedDate).ToList()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("getByFilter"), HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] GetPaymentByFilterRequest request)
        {
            try
            {
                PaymentStatusEnum? statusValue = null;
                if (request.Status.HasValue)
                {
                    if(Enum.IsDefined(typeof(PaymentStatusEnum), request.Status.Value))
                    {
                        statusValue = (PaymentStatusEnum)request.Status.Value;
                    }
                }
                var data = await _uow.PaymentBLL.GetAsync(request.StartDate, request.EndDate, request.FormOfPayment, statusValue, request.PersonCompanyName, request.Page, request.PageSize);
                var total = await _uow.PaymentBLL.GetTotalAsync(request.StartDate, request.EndDate, request.FormOfPayment);
                return Ok(new
                {
                    Data = data.Select(x => new
                    {
                        CreatedDate = x.CreatedDate.AddHours(-3),
                        x.Id,
                        x.Note,
                        x.SoccerPitchReservationId,
                        x.Value,
                        x.PersonCompanyId,
                        PersonCompanyName = x.PersonCompany != null ? x.PersonCompany.Name : "Sem Resp.",
                        FormOfPaymentName = x.FormOfPayment?.Name,
                        StatusDescription = EnumHelper.Instance.GetEnumDescription(x.Status),
                        x.Status
                    }).ToList(),
                    Total = total
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
