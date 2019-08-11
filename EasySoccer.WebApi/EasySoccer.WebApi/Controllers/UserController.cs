using System;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.Entities;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserUoW _uoW;
        public UserController(UserUoW uoW)
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
                return BadRequest(e.ToString());
            }
        }

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
                return BadRequest(e.ToString());
            }
        }
    }
}