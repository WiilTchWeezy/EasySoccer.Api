using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class CompanyScheduleUoW : UoWBase
    {
        public ICompanyScheduleBLL CompanyScheduleBLL { get; set; }
        public CompanyScheduleUoW(IEasySoccerDbContext dbContext, ICompanyScheduleBLL companyScheduleBLL) : base(dbContext)
        {
            CompanyScheduleBLL = companyScheduleBLL;
        }
    }
}
