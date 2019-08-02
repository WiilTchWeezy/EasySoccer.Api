using System;
using System.Linq;
using System.Threading.Tasks;
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
    }
}