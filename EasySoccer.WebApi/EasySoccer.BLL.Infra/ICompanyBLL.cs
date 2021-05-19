using EasySoccer.BLL.Infra.DTO;
using EasySoccer.DAL.Infra.Model;
using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyBLL
    {
        Task<List<CompanyModel>> GetAsync(double? longitude, double? latitude, int page, int pageSize, string name, string orderField, string orderDirection);
        Task<Company> CreateAsync(string name, string description, string cnpj, bool workOnHolidays, double? longitude, double? latitude);
        Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, double? longitude, double? latitude, string completeAddress, List<CompanySchedulesRequest> companySchedules, int? idCity, bool insertReservationConfirmed);
        Task<Company> GetAsync(long companyId);
        Task<CompanyFinancialRecord> GetCurrentFinancialInfoAsync(long companyId);
        Task SaveImageAsync(long companyId, string imageBase64);

        Task SaveFormInputCompanyAsync(FormInputCompanyEntryRequest request);
        Task SaveFormInputContactAsync(FormInputContactRequest request);
        Task ActiveAsync(long companyId, bool active);
        Task<List<City>> GetCitiesByState(int IdState);
        Task<List<State>> GetStates();
        Task SaveFormIndicateCompanyAsync(SaveFormIndicateCompanyRequest request);
    }
}
