using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FinancialController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        [Route("getPlansInfo"), HttpGet]
        public async Task<IActionResult> GetSchedulesResponses()
        {
            try
            {
                var contentPath = _hostingEnvironment.ContentRootPath;
                var text = System.IO.File.ReadAllText(contentPath + @"\Infra\Financial\PlansConfig.json");
                var jArray = JArray.Parse(text);
                return Ok(jArray);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}