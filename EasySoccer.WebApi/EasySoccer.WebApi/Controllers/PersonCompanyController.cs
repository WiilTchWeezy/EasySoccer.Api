using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security.AuthIdentity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonCompanyController : ApiBaseController
    {
        private PersonCompanyUoW _uoW;
        public PersonCompanyController(PersonCompanyUoW uoW) : base(uoW)
        {
            _uoW = uoW;
        }


        [Route("get"), HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetPersonCompanyRequest request)
        {
            try
            {
                var persons = await _uoW.PersonCompanyBLL.GetAsync(request.Name, request.Email, request.Phone, request.Page, request.PageSize, new CurrentUser(HttpContext).CompanyId);
                var totalRegisters = await _uoW.PersonCompanyBLL.GetAsync(request.Name, request.Email, request.Phone, new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    Total = totalRegisters,
                    Data = persons
                    .Select(x => new
                    {
                        x.CompanyId,
                        x.CreatedDate,
                        x.Email,
                        x.Id,
                        x.Name,
                        x.Phone
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("getAutoComplete"), HttpGet]
        public async Task<IActionResult> GetAutoCompleteAsync([FromQuery] string filter)
        {
            try
            {
                return Ok((await _uoW.PersonCompanyBLL.GetAutoCompleteAsync(filter, new CurrentUser(HttpContext).CompanyId)).Select(x => new { name = x.Name + " - (" + x.Phone + ")", Id = x.Id }).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("post"), HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PersonCompanyRequest request)
        {
            try
            {
                var createdPerson = await _uoW.PersonCompanyBLL.CreateAsync(request.Name, request.Email, request.Phone, new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    createdPerson.Id,
                    createdPerson.Name,
                    createdPerson.Email,
                    createdPerson.Phone,
                    createdPerson.CompanyId
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("patch"), HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] PersonCompanyUpdateRequest request)
        {
            try
            {
                var createdPerson = await _uoW.PersonCompanyBLL.UpdateAsync(request.Id, request.Name, request.Email, request.Phone, new CurrentUser(HttpContext).CompanyId);
                return Ok(new
                {
                    createdPerson.Id,
                    createdPerson.Name,
                    createdPerson.Email,
                    createdPerson.Phone,
                    createdPerson.CompanyId
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("getInfo"), HttpGet]
        public async Task<IActionResult> GetInfoAsync([FromQuery] Guid Id)
        {
            try
            {
                var personInfo = await _uoW.PersonCompanyBLL.GetInfoAsync(Id);
                return Ok(personInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
