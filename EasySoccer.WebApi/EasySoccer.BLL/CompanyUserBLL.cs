using EasySoccer.BLL.Exceptions;
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
        private ICompanyFinancialRecordRepository _companyFinancialRecordRepository;
        private IEasySoccerDbContext _dbContext;
        private IUserTokenRepository _userTokenRepository;
        public CompanyUserBLL(ICompanyUserRepository companyUserRepository, IEasySoccerDbContext dbContext, ICompanyFinancialRecordRepository companyFinancialRecordRepository, IUserTokenRepository userTokenRepository)
        {
            _companyUserRepository = companyUserRepository;
            _companyFinancialRecordRepository = companyFinancialRecordRepository;
            _dbContext = dbContext;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
        {
            var currentUser = await _companyUserRepository.GetAsync(userId);
            if (currentUser == null)
                throw new BussinessException("Usuário não encontrado");
            if (!currentUser.Password.Equals(oldPassword))
                throw new BussinessException("A senha antiga não confere");

            currentUser.Password = newPassword;
            await _companyUserRepository.Edit(currentUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyUser> CreateAsync(string name, string email, string phone, string password, long companyId)
        {
            var companyUser = new CompanyUser
            {
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

        public async Task<CompanyUser> GetAsync(long userId)
        {
            return await _companyUserRepository.GetAsync(userId);
        }

        public async Task<UserToken> InsertUserToken(long userId, string token)
        {
            var currentCompanyUser = await _companyUserRepository.GetAsync(userId);
            if (currentCompanyUser == null)
                throw new BussinessException("Usuário não encontrado");
            var currentUserToken = await _userTokenRepository.GetAsync(token, userId);
            if (currentUserToken != null)
            {
                currentUserToken.IsActive = true;
                currentUserToken.LogOffDate = null;
                await _userTokenRepository.Edit(currentUserToken);
                await _dbContext.SaveChangesAsync();
                return currentUserToken;
            }
            var userToken = new UserToken
            {
                Id = new Guid(),
                IsActive = true,
                CompanyUserId = userId,
                CreatedDate = DateTime.Now,
                Token = token,
                TokenType = Entities.Enum.TokenTypeEnum.Mobile
            };
            await _userTokenRepository.Edit(userToken);
            await _dbContext.SaveChangesAsync();
            return userToken;
        }

        public async Task<CompanyUser> LoginAsync(string email, string password)
        {
            var companyUser = await _companyUserRepository.LoginAsync(email, password);
            if (companyUser != null)
            {
                var companyFiscalRecord = await _companyFinancialRecordRepository.GetByCompanyAsync(companyUser.CompanyId);
                if (companyFiscalRecord == null)
                    throw new BussinessException("Erro ao realizar login, por favor verifique as informações financeiras.");
            }

            return companyUser;
        }

        public async Task<CompanyUser> UpdateAsync(long userId, string name, string email, string phone)
        {
            var currentUser = await _companyUserRepository.GetAsync(userId);
            if (currentUser == null)
                throw new BussinessException("Usuário não encontrado.");

            currentUser.Name = name;
            currentUser.Email = email;
            currentUser.Phone = phone.Trim().Replace(")", "").Replace("(", "").Replace("-", "");
            await _companyUserRepository.Edit(currentUser);
            await _dbContext.SaveChangesAsync();
            return currentUser;
        }
    }
}
