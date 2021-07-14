using EasySoccer.BLL.Infra.Services.MessageBird;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.MesssageBird
{
    public class WathsappService : IWathsappService
    {
        private string _apiKey;
        private string _apiUrl;
        private string _channelId;
        public WathsappService(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("MessageBirdConfig");
            if (configSection != null)
            {
                _apiKey = configSection.GetValue<string>("ApiKey");
                _apiUrl = configSection.GetValue<string>("ApiEndPoint");
                _channelId = configSection.GetValue<string>("WathsAppChannelId");
            }
        }
        private HttpClient BuildClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"AccessKey {_apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", $"application/json");
            return httpClient;
        }

        public async Task SendTemplateMessageAsync(string to, string templateName)
        {
            using (var clientHttp = BuildClient())
            {
                var request = new
                {
                    to,
                    type = "hsm",
                    from = _channelId,
                    content = new
                    {
                        hsm = new
                        {
                            Namespace = "8ff3673d_92a4_4ef9_b745_8a192a609839",
                            templateName,
                            language = new
                            {
                                policy = "deterministic",
                                code = "pt-br"
                            }
                        }
                    }
                };
                var requestJson = JsonConvert.SerializeObject(request);
                var httpResponse = await clientHttp.PostAsJsonAsync($"send?access_key={_apiKey}", request);
                if(httpResponse != null && httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
