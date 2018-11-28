using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class CompanyUoW : UoWBase
    {
        public ICompanyBLL CompanyBLL { get; set; }
        public CompanyUoW(IEasySoccerDbContext dbContext, ICompanyBLL companyBLL) : base(dbContext)
        {
            CompanyBLL = companyBLL;
        }
    }
}
