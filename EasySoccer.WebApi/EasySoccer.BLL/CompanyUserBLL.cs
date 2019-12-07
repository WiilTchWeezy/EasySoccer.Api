using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyUserBLL : ICompanyUserBLL
    {
        private ICompanyUserRepository _companyUserRepository;
        private IEasySoccerDbContext _dbContext;
        public CompanyUserBLL(ICompanyUserRepository companyUserRepository, IEasySoccerDbContext dbContext)
        {
            _companyUserRepository = companyUserRepository;
            _dbContext = dbContext;
        }

        public async Task<CompanyUser> CreateAsync(string name, string email, string phone, string password, long companyId)
        {
            var companyUser = new CompanyUser {
                CompanyId = companyId,
                CreatedDate = DateTime.UtcNow,
                Email = email, 
                Name = name,
                Password = password,
                Phone = phone
            };
            await _companyUserRepository.Create<CompanyUser>(companyUser);
            await _dbContext.SaveChangesAsync();
            return companyUser;
        }

        public Task<CompanyUser> LoginAsync(string email, string password)
        {
            return _companyUserRepository.LoginAsync(email, password);
        }


    }
}
