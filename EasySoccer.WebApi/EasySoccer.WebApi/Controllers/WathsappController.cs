using EasySoccer.BLL.Infra.Services.MessageBird;
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
        private IWathsappService _wathsappService;
        public WathsappController(WathsappUoW uoW, IWathsappService wathsappService) : base(uoW)
        {
            _uow = uoW;
            _wathsappService = wathsappService;
        }

        [AllowAnonymous]
        [Route("startconversation"), HttpPost]
        public async Task<IActionResult> StartConversationAsync()
        {
            try
            {
                await _wathsappService.SendTemplateMessageAsync("+5516991255409", "sample_shipping_confirmation");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());

            }
        }
    }
}
