using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyBLL
    {
        Task<List<Company>> GetAsync(double? longitude, double? latitude, string description, int page, int pageSize);
        Task<Company> CreateAsync(string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude);
        Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude);
    }
}
