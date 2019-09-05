using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyUserBLL : ICompanyUserBLL
    {
        private ICompanyUserRepository _companyUserRepository;
        public CompanyUserBLL(ICompanyUserRepository companyUserRepository)
        {
            _companyUserRepository = companyUserRepository;
        }
        public Task<CompanyUser> LoginAsync(string email, string password)
        {
            return _companyUserRepository.LoginAsync(email, password);
        }
    }
}
