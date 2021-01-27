using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class CompanyUserNotificationRepository : RepositoryBase, ICompanyUserNotificationRepository
    {
        public CompanyUserNotificationRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<CompanyUserNotification>> GetAsync(long companyUserId, int page = 1, int pageSize = 10)
        {
            return _dbContext.CompanyUserNotificationQuery.Where(x => x.IdCompanyUser == companyUserId && (x.Active == null || x.Active == true)).OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<CompanyUserNotification> GetAsync(long companyUserId, DateTime date, NotificationTypeEnum notificationType)
        {
            return _dbContext.CompanyUserNotificationQuery.Where(x => x.IdCompanyUser == companyUserId && x.CreatedDate.Date > date.Date && x.NotificationType == notificationType && (x.Active == null || x.Active == true)).FirstOrDefaultAsync();
        }

        public Task<List<CompanyUserNotification>> GetAsync(long companyId, NotificationTypeEnum notificationType)
        {
            return _dbContext.CompanyUserNotificationQuery.Include(x => x.CompanyUser).Where(x => x.CompanyUser.CompanyId == companyId && x.NotificationType == notificationType).ToListAsync();
        }
    }
}
