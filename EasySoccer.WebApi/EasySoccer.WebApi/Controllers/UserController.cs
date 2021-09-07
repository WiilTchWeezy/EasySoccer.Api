using System;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiBaseController
    {
        private UserUoW _uoW;
        public UserController(UserUoW uoW) : base(uoW)
        {
            _uoW = uoW;
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]string filter)
        {
            try
            {
                return Ok((await _uoW.UserBLL.GetAsync(filter)).Select(x => new { name = x.Name + " - (" + x.Phone + ")", Id = x.PersonId }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [Route("createPerson"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]UserRequest userRequest)
        {
            try
            {
                Entities.Enum.CreatedFromEnum enumValue;
                if (userRequest.CreatedFrom.HasValue)
                    enumValue = userRequest.CreatedFrom.Value;
                else
                    enumValue = Entities.Enum.CreatedFromEnum.WebApp;
                var userCreated = await _uoW.UserBLL.CreatePersonAsync(userRequest.Name, userRequest.PhoneNumber, userRequest.Email, enumValue);
                return Ok(new
                {
                    userCreated.PersonId,
                    userCreated.Email,
                    userCreated.Name,
                    userCreated.Phone
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("changepassword"), HttpGet]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]UserChangePasswordRequest request)
        {
            try
            {
                if (await _uoW.UserBLL.ChangeUserPassword(request.OldPassword, new MobileUser(HttpContext).UserId, request.NewPassword))
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("getInfo"), HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            try
            {
                var user = await _uoW.UserBLL.GetAsync(new MobileUser(HttpContext).UserId);
                return Ok(new { Id = user.UserId, user.Name, user.Phone, user.Email });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("patch"), HttpPost]
        public async Task<IActionResult> UpdateUserAsync([FromBody]UserRequest request)
        {
            try
            {
                var user = await _uoW.UserBLL.UpdateAsync(new MobileUser(HttpContext).UserId, request.Name, request.Email, request.PhoneNumber);
                return Ok(new { user.UserId, user.Name, user.Phone, user.Email });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [Route("createUser"), HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]UserRequest userRequest)
        {
            try
            {
                var createdFrom = CreatedFromEnum.Mobile;
                if (userRequest != null && userRequest.CreatedFrom.HasValue)
                    createdFrom = userRequest.CreatedFrom.Value;
                var userCreated = await _uoW.UserBLL.CreateUserAsync(userRequest.Name, userRequest.PhoneNumber, userRequest.Email, userRequest.Password, createdFrom);
                return Ok(new
                {
                    userCreated.PersonId,
                    userCreated.Email,
                    userCreated.Name,
                    userCreated.Phone
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("inserttoken"), HttpPost]
        public async Task<IActionResult> InsertTokenAsync([FromBody] UserTokenRequest request)
        {
            try
            {
                var userToken = await _uoW.UserBLL.InsertUserToken(new MobileUser(HttpContext).UserId, request.Token);
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
                await _uoW.UserBLL.LogOffUserToken(request.UserId, request.Token);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("requestresetpassword"), HttpPost]
        public async Task<IActionResult> RequestResetPasswordAsync([FromBody] RequestPasswordResetRequest request)
        {
            try
            {
                await _uoW.UserBLL.UserRequestResetPasswordAsync(request.Email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("resetpassword"), HttpPost]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            try
            {
                await _uoW.UserBLL.ResetUserPasswordAsync(request.Token, request.Password);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}