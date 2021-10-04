using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
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
    public class CompanyUserController : ApiBaseController
    {
        private CompanyUserUoW _companyUserUow;
        public CompanyUserController(CompanyUserUoW companyUserUow) : base(companyUserUow)
        {
            _companyUserUow = companyUserUow;
        }

        [AllowAnonymous]
        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CompanyUserRequest request)
        {
            try
            {
                return Ok(await _companyUserUow.CompanyUserBLL.CreateAsync(request.Name, request.Email, request.Phone, request.Password, request.CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("changepassword"), HttpPost]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePasswordRequest request)
        {
            try
            {
                return Ok(await _companyUserUow.CompanyUserBLL.ChangePasswordAsync(new CurrentUser(HttpContext).UserId, request.OldPassword, request.NewPassword));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("getInfo"), HttpGet]
        public async Task<IActionResult> GetCompanyUserInformationAsync()
        {
            try
            {
                var companyUser = await _companyUserUow.CompanyUserBLL.GetAsync(new CurrentUser(HttpContext).UserId);
                return Ok(new
                {
                    companyUser?.Email,
                    companyUser?.Name,
                    companyUser?.Phone,
                    companyUser?.CompanyId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] CompanyUserRequest request)
        {
            try
            {
                var companyUser = await _companyUserUow.CompanyUserBLL.UpdateAsync(new CurrentUser(HttpContext).UserId, request.Name, request.Email, request.Phone);
                return Ok(new
                {
                    companyUser?.Email,
                    companyUser?.Name,
                    companyUser?.Phone,
                    companyUser?.CompanyId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("inserttoken"), HttpPost]
        public async Task<IActionResult> InsertTokenAsync([FromBody] UserTokenRequest request)
        {
            try
            {
                var userToken = await _companyUserUow.CompanyUserBLL.InsertUserToken(new CurrentUser(HttpContext).UserId, request.Token);
                return Ok(userToken);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("logofftoken"), HttpPost]
        public async Task<IActionResult> LogOffTokenAsync([FromBody] UserTokenRequest request)
        {
            try
            {
                await _companyUserUow.CompanyUserBLL.LogOffUserToken(request.CompanyUserId, request.Token);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("getNotifications"), HttpGet]
        public async Task<IActionResult> GetNotificationsAsync()
        {
            try
            {
                var companyUserNoitications = await _companyUserUow.CompanyUserBLL.GetCompanyUserNotificationsAsync(new CurrentUser(HttpContext).UserId);
                return Ok(companyUserNoitications);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("payment"), HttpPost]
        public async Task<IActionResult> PaymentAsync([FromBody] BLL.Infra.Services.PaymentGateway.Request.GatewayPaymentRequest request)
        {
            try
            {                
                return Ok(await _companyUserUow.CompanyUserBLL.PayAsync(request, new CurrentUser(HttpContext).UserId, new CurrentUser(HttpContext).CompanyId));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}