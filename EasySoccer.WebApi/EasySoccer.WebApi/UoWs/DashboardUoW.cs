using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class DashboardUoW : UoWBase
    {
        public ISoccerPitchReservationBLL SoccerPitchReservationBLL { get; set; }
        public DashboardUoW(IEasySoccerDbContext dbContext, ISoccerPitchReservationBLL soccerPitchReservationBLL) : base(dbContext)
        {
            SoccerPitchReservationBLL = soccerPitchReservationBLL;
        }
    }
}
