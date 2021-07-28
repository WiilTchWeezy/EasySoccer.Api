using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class CompanyUoW : UoWBase
    {
        public ICompanyBLL CompanyBLL { get; set; }
        public ISoccerPitchBLL SoccerPitchBLL { get; set; }
        public ISoccerPitchReservationBLL SoccerPitchReservationBLL { get; set; }
        public CompanyUoW(IEasySoccerDbContext dbContext, ICompanyBLL companyBLL, ISoccerPitchBLL soccerPitchBLL, ISoccerPitchReservationBLL soccerPitchReservationBLL) : base(dbContext)
        {
            CompanyBLL = companyBLL;
            SoccerPitchBLL = soccerPitchBLL;
            SoccerPitchReservationBLL = soccerPitchReservationBLL;
        }
    }
}
