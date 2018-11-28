using EasySoccer.BLL.Helper;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyBLL : ICompanyBLL
    {
        private ICompanyRepository _companyRepository;
        public CompanyBLL(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<Company>> GetAsync(double? longitude, double? latitude, string description, int page, int pageSize)
        {
            var companies = await _companyRepository.GetAsync(description, page, pageSize);

            if (longitude.HasValue && latitude.HasValue)
            {
                foreach (var item in companies)
                {
                    item.Distance = LocationHelper.Haversine(longitude.Value, latitude.Value, (double)item.Longitude, (double)item.Latitude);
                }
                return companies.OrderBy(c => c.Distance).ToList();
            }
            return companies;
        }
    }
}
