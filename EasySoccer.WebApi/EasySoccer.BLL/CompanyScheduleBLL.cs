using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyScheduleBLL : ICompanyScheduleBLL
    {
        private ICompanyScheduleRepository _companyScheduleRepository;
        public CompanyScheduleBLL(ICompanyScheduleRepository companyScheduleRepository)
        {
            _companyScheduleRepository = companyScheduleRepository;
        }
        public Task<CompanySchedule> GetCompanyScheduleByDay(int companyId, int dayOfWeek)
        {
            return _companyScheduleRepository.GetAsync(companyId, dayOfWeek);
        }
    }
}
