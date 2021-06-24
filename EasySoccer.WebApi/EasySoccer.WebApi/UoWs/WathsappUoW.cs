using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class WathsappUoW : UoWBase
    {
        public ICompanyBLL CompanyBLL { get; set; }
        public WathsappUoW(IEasySoccerDbContext dbContext, ICompanyBLL companyBLL) : base(dbContext)
        {
            CompanyBLL = companyBLL;
        }
    }
}
