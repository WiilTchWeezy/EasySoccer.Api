using EasySoccer.BLL.Infra.Services.PushNotification;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.PushNotification
{
    public class NotificationService : INotificationService
    {
        private string _key = "";
        private string _fcmApiUrl = "";

        public NotificationService(IConfiguration configuration)
        {
            var config = configuration.GetSection("GeneralConfig");
            if (config != null)
            {
                _key = config.GetValue<string>("FCMKey");
                _fcmApiUrl = config.GetValue<string>("FCMApiUrl");
            }
        }

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_fcmApiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            if (string.IsNullOrEmpty(_key) == false)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "key=" + _key);
            }
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            return httpClient;
        }

        public async Task SendNotification(string tokenTo, object payloadData)
        {
            using (var httpClient = CreateClient())
            {
                var requestObj = new
                {
                    to = tokenTo,
                    data = payloadData
                };
                await httpClient.PostAsJsonAsync("fcm/send", requestObj);
            }
        }
    }
}
