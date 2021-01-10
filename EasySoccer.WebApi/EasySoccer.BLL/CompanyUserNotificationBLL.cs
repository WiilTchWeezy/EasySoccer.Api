using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.Services.PushNotification;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyUserNotificationBLL : ICompanyUserNotificationBLL
    {
        private INotificationService _notificationService;
        private ICompanyUserNotificationRepository _companyUserNotificationRepository;
        private IEasySoccerDbContext _dbContext;
        public CompanyUserNotificationBLL(INotificationService notificationService, ICompanyUserNotificationRepository companyUserNotificationRepository, IEasySoccerDbContext dbContext)
        {
            _notificationService = notificationService;
            _companyUserNotificationRepository = companyUserNotificationRepository;
            _dbContext = dbContext;
        }

        public async Task<CompanyUserNotification> CreateCompanyUserNotificationAsync(long companyUserId, string title, string message, string token, NotificationTypeEnum notificationTypeEnum)
        {
            var companyUserNotification = new CompanyUserNotification
            {
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                IdCompanyUser = companyUserId,
                Message = message,
                NotificationType = notificationTypeEnum,
                Read = false,
                Title = title
            };
            await _companyUserNotificationRepository.Create(companyUserNotification);
            await _dbContext.SaveChangesAsync();
            var dic = new Dictionary<string, string>();
            dic.Add("title", title);
            dic.Add("message", message);
            await _notificationService.SendNotification(token, dic);
            return companyUserNotification;
        }
    }
}
