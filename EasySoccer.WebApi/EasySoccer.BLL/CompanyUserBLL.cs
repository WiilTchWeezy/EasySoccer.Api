using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.Helpers;
using EasySoccer.BLL.Infra.Services.PaymentGateway;
using EasySoccer.BLL.Infra.Services.PaymentGateway.Request;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyUserBLL : ICompanyUserBLL
    {
        private ICompanyUserRepository _companyUserRepository;
        private ICompanyFinancialRecordRepository _companyFinancialRecordRepository;
        private IEasySoccerDbContext _dbContext;
        private IUserTokenRepository _userTokenRepository;
        private ICompanyUserNotificationRepository _companyUserNotificationRepository;
        private IPaymentGatewayService _paymentGateWayService;
        private IStateRepository _stateRepository;
        private ICityRepository _cityRepository;
        public CompanyUserBLL
            (ICompanyUserRepository companyUserRepository,
            IEasySoccerDbContext dbContext,
            ICompanyFinancialRecordRepository companyFinancialRecordRepository,
            IUserTokenRepository userTokenRepository,
            ICompanyUserNotificationRepository companyUserNotificationRepository,
            IPaymentGatewayService paymentGateWayService,
            IStateRepository stateRepository,
            ICityRepository cityRepository)
        {
            _companyUserRepository = companyUserRepository;
            _companyFinancialRecordRepository = companyFinancialRecordRepository;
            _dbContext = dbContext;
            _userTokenRepository = userTokenRepository;
            _companyUserNotificationRepository = companyUserNotificationRepository;
            _paymentGateWayService = paymentGateWayService;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
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

        public Task<List<CompanyUserNotification>> GetCompanyUserNotificationsAsync(long companyUserId, int page = 1, int pageSize = 10)
        {
            return _companyUserNotificationRepository.GetAsync(companyUserId, page, pageSize);
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
            await _userTokenRepository.Create(userToken);
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

        public async Task<UserToken> LogOffUserToken(long userId, string token)
        {
            var userToken = await _userTokenRepository.GetAsync(token, userId);
            if (userToken == null)
                throw new BussinessException("Usuário não encontrado.");
            userToken.IsActive = false;
            userToken.LogOffDate = DateTime.UtcNow;
            await _userTokenRepository.Edit(userToken);
            await _dbContext.SaveChangesAsync();
            return userToken;
        }

        public async Task<CompanyFinancialRecord> PayAsync(PaymentRequest request, long companyUserId, long companyId)
        {
            var companyUser = await _companyUserRepository.GetAsync(companyUserId);
            if (companyUser == null)
                throw new BussinessException("Usuário não encontrado.");
            var planValue = FinancialHelper.Instance.GetValueFromPlan((FinancialPlanEnum)request.SelectedPlan);
            var installment = request.SelectedInstallments > 12 ? 1 : request.SelectedInstallments;
            var city = await _cityRepository.GetAsync(request.CityId);
            if (city == null)
                throw new BussinessException("Cidade não encontrada.");
            var state = await _stateRepository.GetAsync(request.StateId);
            if (state == null)
                throw new BussinessException("Estado não encontrado.");

            var transaction = await _paymentGateWayService.PayAsync(request, companyUser, planValue, installment, state.Code, city.Name);
            if (transaction != null)
            {
                if (transaction.IsAuthorized)
                {
                    var currentFinancialRecord = await _companyFinancialRecordRepository.GetByCompanyAsync(companyId);
                    DateTime expiresDate = DateTime.UtcNow;
                    if (currentFinancialRecord != null)
                        expiresDate = currentFinancialRecord.ExpiresDate;
                    var companyFinancialRecord = new CompanyFinancialRecord()
                    {
                        FinancialPlan = (FinancialPlanEnum)request.SelectedPlan,
                        CompanyId = companyId,
                        CreatedDate = DateTime.UtcNow,
                        ExpiresDate = expiresDate.AddMonths(FinancialHelper.Instance.GetMonthsFromPlan((FinancialPlanEnum)request.SelectedPlan)),
                        Paid = transaction.IsAuthorized,
                        Transaction = transaction.TransactionJson,
                        Value = planValue
                    };
                    await _companyFinancialRecordRepository.Create(companyFinancialRecord);
                    var userNotifications = await _companyUserNotificationRepository.GetAsync(companyId, NotificationTypeEnum.FinancialRenewal);
                    if(userNotifications != null)
                    {
                        foreach (var item in userNotifications)
                        {
                            item.Active = false;
                            await _companyUserNotificationRepository.Edit(item);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return companyFinancialRecord;
                }
                else
                {
                    throw new BussinessException("Não foi possível realizar o pagamento.");
                }
            }
            return null;
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
