using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.PushNotification
{
    public interface INotificationService
    {
        Task SendNotification(string tokenTo, object payloadData);
    }
}
