using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyBLL
    {
        Task<List<Company>> GetAsync(double? longitude, double? latitude, int page, int pageSize, string name, string orderField, string orderDirection);
        Task<Company> CreateAsync(string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude);
        Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude, string completeAddress, List<CompanySchedulesRequest> companySchedules);
        Task<Company> GetAsync(long companyId);
        Task SaveImageAsync(long companyId, string imageBase64);

        Task SaveFormInputCompanyAsync(FormInputCompanyEntryRequest request);
        Task SaveFormInputContactAsync(FormInputContactRequest request);
    }
}
