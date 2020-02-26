using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Helper;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
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
        private IEasySoccerDbContext _dbContext;
        public CompanyBLL(ICompanyRepository companyRepository, IEasySoccerDbContext dbContext)
        {
            _companyRepository = companyRepository;
            _dbContext = dbContext;
        }

        public async Task<Company> CreateAsync(string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude)
        {
            var company = new Company
            {
                CNPJ = cnpj,
                CreatedDate = DateTime.UtcNow,
                Description = description,
                Latitude = latitude,
                Longitude = longitude,
                Name = name,
                WorkOnHoliDays = workOnHolidays
            };

            await _companyRepository.Create(company);
            await _dbContext.SaveChangesAsync();
            return company;
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

        public Task<Company> GetAsync(long companyId)
        {
            return _companyRepository.GetAsync(companyId);
        }

        public async Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude, string completeAddress)
        {
            var currentCompany = await _companyRepository.GetAsync(id);
            if (currentCompany == null)
                throw new NotFoundException(currentCompany, id);
            currentCompany.Name = name;
            currentCompany.Description = description;
            currentCompany.CNPJ = cnpj;
            currentCompany.CompleteAddress = completeAddress;
            currentCompany.WorkOnHoliDays = workOnHolidays;
            if (longitude.HasValue)
                currentCompany.Longitude = longitude;
            if (latitude.HasValue)
                currentCompany.Latitude = latitude;
            await _companyRepository.Edit(currentCompany);
            await _dbContext.SaveChangesAsync();
            return currentCompany;

        }
    }
}
