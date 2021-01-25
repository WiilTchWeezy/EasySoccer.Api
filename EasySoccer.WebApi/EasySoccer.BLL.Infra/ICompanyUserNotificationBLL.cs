using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyUserNotificationBLL
    {
        Task<CompanyUserNotification> CreateCompanyUserNotificationAsync(long companyUserId, string title, string message, NotificationTypeEnum notificationTypeEnum, string data);
    }
}
