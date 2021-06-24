using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WathsappController : ApiBaseController
    {
        private WathsappUoW _uow;
        public WathsappController(WathsappUoW uoW) : base(uoW)
        {
            _uow = uoW;
        }

        [AllowAnonymous]
        [Route("startconversation"), HttpPost]
        public async Task<IActionResult> StartConversationAsync([FromBody] WathsappRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                await _uow.CompanyBLL.StartConversationAsync(json);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());

            }
        }
    }
}
