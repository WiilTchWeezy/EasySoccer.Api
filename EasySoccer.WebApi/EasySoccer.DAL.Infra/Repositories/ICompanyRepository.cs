using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyRepository: IRepositoryBase
    {
        Task<List<Company>> GetAsync( int page, int pageSize, string name, string orderField, string orderDirection);
        Task<Company> GetAsync(long id);
        Task<Company> GetAsync(string companyDocument);
    }
}
