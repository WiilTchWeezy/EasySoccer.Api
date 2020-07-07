using EasySoccer.BLL.Infra.Services.SendGrid;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.SendGrid
{
    public class EmailService : IEmailService
    {
        private string _apiKey = "";
        private string _emailFrom = "";
        private string _userFrom = "";
        private string _validationErrorTemplateId = "";
        private string _successTemplateId = "";
        public EmailService(IConfiguration configuration)
        {
            var config = configuration.GetSection("SendGridConfiguration");
            if (config != null)
            {
                _apiKey = config.GetValue<string>("ApiKey");
                _emailFrom = config.GetValue<string>("EmailFrom");
                _userFrom = config.GetValue<string>("UserNameFrom");
                _validationErrorTemplateId = config.GetValue<string>("ValidationErrorTemplateId");
                _successTemplateId = config.GetValue<string>("SuccessCompanyEntryTemplateId");
            }
        }
        private async Task SendEmailAsync(string emailTo, string userNameTo, string templateId, object templateData)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_emailFrom, _userFrom);
            var to = new EmailAddress(emailTo, userNameTo);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);
            var response = await client.SendEmailAsync(msg);
            var contentResponse = await response.Body.ReadAsStringAsync();
        }

        public async Task SendValidationErrorsEmailAsync(string emailTo, string userNameTo, string errors)
        {
            await SendEmailAsync(emailTo, userNameTo, _validationErrorTemplateId, new { firstName = userNameTo, errors = errors });
        }

        public async Task SendSuccessEmailAsync(string emailTo, string userNameTo, int daysFree, string password)
        {
            await SendEmailAsync(emailTo, userNameTo, _successTemplateId, new { firstName = userNameTo, daysFree, user = emailTo, password });
        }

        public async Task SendTextEmailAsync(string emailTo, string userNameTo, string subject,string text)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_emailFrom, _userFrom);
            var to = new EmailAddress(emailTo, userNameTo);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, text, null);
            var response = await client.SendEmailAsync(msg);
            var contentResponse = await response.Body.ReadAsStringAsync();
        }
    }
}
