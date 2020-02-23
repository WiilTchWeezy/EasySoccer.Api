using EasySoccer.Entities;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyUserRepository : IRepositoryBase
    {
        Task<CompanyUser> LoginAsync(string email, string password);
        Task<CompanyUser> GetAsync(long userId);
    }
}
