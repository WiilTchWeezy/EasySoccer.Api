using EasySoccer.BLL.Infra.Services.PaymentGateway.Request;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyUserBLL
    {
        Task<CompanyUser> LoginAsync(string email, string password);
        Task<CompanyUser> CreateAsync(string name, string email, string phone, string password, long companyId);
        Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword);
        Task<CompanyUser> GetAsync(long userId);
        Task<CompanyUser> UpdateAsync(long userId, string name, string email, string phone);

        Task<UserToken> InsertUserToken(long userId, string token);
        Task<UserToken> LogOffUserToken(long userId, string token);
        Task<List<CompanyUserNotification>> GetCompanyUserNotificationsAsync(long companyUserId, int page = 1, int pageSize = 10);

        Task<CompanyFinancialRecord> PayAsync(GatewayPaymentRequest request, long companyUserId, long companyId);
    }
}
