using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.SendGrid
{
    public interface  IEmailService
    {
        Task SendValidationErrorsEmailAsync(string emailTo, string userNameTo, string[] errors);
    }
}
