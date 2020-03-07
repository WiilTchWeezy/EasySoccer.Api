using System;
using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CompanyController : ApiBaseController
    {
        private CompanyUoW _uow;
        public CompanyController(CompanyUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [AllowAnonymous]
        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]GetCompanyRequest request)
        {
            return Ok(await _uow.CompanyBLL.GetAsync(request.Longitude, request.Latitude, request.Description, request.Page, request.PageSize));
        }

        [AllowAnonymous]
        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]CompanyRequest request)
        {
            try
            {
                return Ok(await _uow.CompanyBLL.CreateAsync(request.Name, request.Description, request.CNPJ, request.WorkOnHolidays, request.Longitude, request.Latitude));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]CompanyRequest request)
        {
            try
            {
                return Ok(await _uow.CompanyBLL.UpdateAsync(request.Id, request.Name, request.Description, request.CNPJ, request.WorkOnHolidays, request.Longitude, request.Latitude, request.CompleteAddress));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("getcompanyinfo"), HttpGet]
        public async Task<IActionResult> GetCompanyInfo()
        {
            try
            {
                var currentCompany = await _uow.CompanyBLL.GetAsync(new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    currentCompany?.Name,
                    currentCompany?.Description,
                    currentCompany?.CompleteAddress,
                    currentCompany?.CNPJ
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("patchcompanyinfo"), HttpPatch]
        public async Task<IActionResult> UpdateCompanyInfoAsync([FromBody]CompanyRequest request)
        {
            try
            {
                return Ok(await _uow.CompanyBLL.UpdateAsync(new CurrentUser(HttpContext).CompanyId, request.Name, request.Description, request.CNPJ, request.WorkOnHolidays, request.Longitude, request.Latitude, request.CompleteAddress));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}