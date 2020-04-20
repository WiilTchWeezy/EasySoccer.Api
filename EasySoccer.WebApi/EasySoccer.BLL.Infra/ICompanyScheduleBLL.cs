using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyScheduleBLL
    {
        Task<CompanySchedule> GetCompanyScheduleByDay(int companyId, int dayOfWeek);
    }
}
