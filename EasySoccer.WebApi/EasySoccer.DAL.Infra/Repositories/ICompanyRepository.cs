using EasySoccer.DAL.Infra.Model;
using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyRepository : IRepositoryBase
    {
        Task<List<CompanyModel>> GetAsync(int page, int pageSize, string name, string orderField, string orderDirection, double? longitude, double? lattitude, int idCity, int idState);
        Task<Company> GetAsync(long id);
        Task<Company> GetAsync(string companyDocument);
    }
}
