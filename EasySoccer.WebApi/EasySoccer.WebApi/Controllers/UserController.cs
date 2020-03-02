using System;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.Entities;
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
        public UserController(UserUoW uoW) : base (uoW)
        {
            _uoW = uoW;
        }

        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]string filter)
        {
            try
            {
                return Ok((await _uoW.UserBLL.GetAsync(filter)).Select(x => new { name = x.Name + " - (" + x.Phone + ")", x.Id }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [Route("create"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]UserRequest userRequest)
        {
            try
            {
                var userCreated = await _uoW.UserBLL.CreateAsync(new Entities.User
                {
                    Email = userRequest.Email,
                    Name = userRequest.Name,
                    Phone = userRequest.PhoneNumber,
                    SocialMediaId = userRequest.SocialMediaId,
                    Password = userRequest.Password
                });
                return Ok(new
                {
                    userCreated.Id,
                    userCreated.CreatedDate,
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
                return Ok(new { user.Id, user.Name, user.Phone, user.Email });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}