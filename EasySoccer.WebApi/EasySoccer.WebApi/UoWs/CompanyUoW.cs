using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class CompanyUoW : UoWBase
    {
        public ICompanyBLL CompanyBLL { get; set; }
        public ISoccerPitchBLL SoccerPitchBLL { get; set; }
        public CompanyUoW(IEasySoccerDbContext dbContext, ICompanyBLL companyBLL, ISoccerPitchBLL soccerPitchBLL) : base(dbContext)
        {
            CompanyBLL = companyBLL;
            SoccerPitchBLL = soccerPitchBLL;
        }
    }
}
