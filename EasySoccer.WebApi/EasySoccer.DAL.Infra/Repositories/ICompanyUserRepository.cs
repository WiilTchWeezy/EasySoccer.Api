using EasySoccer.Entities;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyUserRepository
    {
        Task<CompanyUser> LoginAsync(string email, string password);
    }
}
