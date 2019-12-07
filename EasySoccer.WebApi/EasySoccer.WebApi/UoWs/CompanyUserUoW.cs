using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class CompanyUserUoW : UoWBase
    {
        public ICompanyUserBLL CompanyUserBLL { get; set; }
        public CompanyUserUoW(IEasySoccerDbContext dbContext, ICompanyUserBLL companyUserBLL) : base(dbContext)
        {
            CompanyUserBLL = companyUserBLL;
        }
    }
}
