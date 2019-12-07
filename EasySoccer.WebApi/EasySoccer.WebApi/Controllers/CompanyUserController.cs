using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
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
        public async Task<IActionResult> PostAsync([FromBody]CompanyUserRequest request)
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

    }
}