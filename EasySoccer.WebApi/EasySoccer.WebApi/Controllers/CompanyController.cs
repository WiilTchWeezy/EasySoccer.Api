using System;
using System.Linq;
using System.Threading.Tasks;
using EasySoccer.BLL.Infra.DTO;
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
            return Ok(await _uow.CompanyBLL.GetAsync(request.Longitude, request.Latitude, request.Page, request.PageSize, request.Name, request.OrderField, request.OrderDirection));
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
                    currentCompany?.CNPJ,
                    currentCompany?.Logo,
                    currentCompany?.IdCity,
                    IdState = currentCompany.City != null ? currentCompany.City.IdState : 0,
                    State = currentCompany.City != null && currentCompany.City.State != null ? currentCompany.City.State.Name : string.Empty,
                    City = currentCompany.IdCity.HasValue ? currentCompany?.City.Name : string.Empty,
                    currentCompany?.Active,
                    Longitude = currentCompany?.Longitude != (decimal)0.00 ? currentCompany?.Longitude : null,
                    Latitude = currentCompany?.Latitude != (decimal)0.00 ? currentCompany?.Latitude : null,
                    CompanySchedules = currentCompany?.CompanySchedules?.Select(x => new
                    {
                        x.CompanyId,
                        x.Day,
                        x.FinalHour,
                        x.StartHour,
                        x.WorkOnThisDay
                    }).ToList()
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
                await _uow.CompanyBLL.UpdateAsync(new CurrentUser(HttpContext).CompanyId, request.Name, request.Description, request.CNPJ, request.WorkOnHolidays, request.Longitude, request.Latitude, request.CompleteAddress, request.CompanySchedules, request.IdCity);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("saveimage"), HttpPost]
        public async Task<IActionResult> SaveImageAsync([FromBody]CompanyImageRequest request)
        {
            try
            {
                await _uow.CompanyBLL.SaveImageAsync(new CurrentUser(HttpContext).CompanyId, request.ImageBase64);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("companyforminput"), HttpPost]
        public async Task<IActionResult> CompanyFormInputAsync([FromBody]FormInputCompanyEntryRequest request)
        {
            try
            {
                await _uow.CompanyBLL.SaveFormInputCompanyAsync(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("contactforminput"), HttpPost]
        public async Task<IActionResult> ContactFormInputAsync([FromBody]FormInputContactRequest request)
        {
            try
            {
                await _uow.CompanyBLL.SaveFormInputContactAsync(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [Route("active"), HttpPost]
        public async Task<IActionResult> ActiveAsync([FromBody]CompanyActiveRequest request)
        {
            try
            {
                await _uow.CompanyBLL.ActiveAsync(new CurrentUser(HttpContext).CompanyId, request.Active);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [AllowAnonymous]
        [Route("getcitiesbystate"), HttpGet]
        public async Task<IActionResult> GetCitiesByStateAsync([FromQuery]int IdState)
        {
            try
            {
                return Ok(await _uow.CompanyBLL.GetCitiesByState(IdState));
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("getstates"), HttpGet]
        public async Task<IActionResult> GetStateAsync()
        {
            try
            {
                return Ok(await _uow.CompanyBLL.GetStates());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}