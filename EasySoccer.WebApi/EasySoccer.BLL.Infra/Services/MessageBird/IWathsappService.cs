using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.MessageBird
{
    public interface IWathsappService
    {
        Task SendTemplateMessageAsync(string to, string templateName);
    }
}
