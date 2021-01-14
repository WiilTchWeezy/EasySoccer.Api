using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyUserNotificationRepository : IRepositoryBase
    {
        Task<List<CompanyUserNotification>> GetAsync(long companyUserId, int page = 1, int pageSize = 10);
        Task<CompanyUserNotification> GetAsync(long companyUserId, DateTime utcNow);
    }
}
