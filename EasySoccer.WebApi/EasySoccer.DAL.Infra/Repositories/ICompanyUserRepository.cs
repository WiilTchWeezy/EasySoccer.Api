using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyUserRepository : IRepositoryBase
    {
        Task<CompanyUser> LoginAsync(string email, string password);
        Task<CompanyUser> GetAsync(long userId);
        Task<CompanyUser> GetAsync(string userEmail);
        Task<List<CompanyUser>> GetByCompanyIdAsync(long cpmpanyId);
    }
}
