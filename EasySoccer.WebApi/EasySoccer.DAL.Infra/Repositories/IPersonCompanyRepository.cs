using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IPersonCompanyRepository : IRepositoryBase
    {
        Task<PersonCompany> GetAsync(Guid id);
        Task<List<PersonCompany>> GetAsync(string filter, long companyId);
        Task<List<PersonCompany>> GetAsync(long companyId);
        Task<PersonCompany> GetAsync(string email, string phone, long companyId);
        Task<PersonCompany> GetByEmailAsync(string email, long companyId);
        Task<PersonCompany> GetByPhoneAsync(string phone, long companyId);
        Task<List<PersonCompany>> GetAsync(string name, string email, string phone, int page, int pageSize, long companyId);
        Task<PersonCompany> GetByPersonIdAsync(Guid personId);
        Task<int> GetAsync(string name, string email, string phone, long companyId);
    }
}
