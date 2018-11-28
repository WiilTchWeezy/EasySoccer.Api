using System.Threading.Tasks;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Mvc;

namespace EasySoccer.WebApi.Controllers
{
    [Produces("application/json")]
    public class CompanyController : ApiBaseController
    {
        private CompanyUoW _uow;
        public CompanyController(CompanyUoW uow) : base(uow)
        {
            _uow = uow;
        }

        [Route("api/company/get"), HttpGet]
        public async Task<IActionResult> GetDonatedAnimals([FromQuery]GetCompanyRequest request)
        {
            return Ok(await _uow.CompanyBLL.GetAsync(request.Longitude, request.Latitude, request.Description, request.Page, request.PageSize));
        }

    }
}