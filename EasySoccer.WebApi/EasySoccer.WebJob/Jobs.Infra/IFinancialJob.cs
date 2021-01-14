using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.WebJob.Jobs.Infra
{
    public interface IFinancialJob
    {
        Task GenerateNotificationsToFinancialRecords();
    }
}
