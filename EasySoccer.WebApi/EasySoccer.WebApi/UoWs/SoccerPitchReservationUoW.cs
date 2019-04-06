using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class SoccerPitchReservationUoW : UoWBase
    {
        public ISoccerPitchReservationBLL SoccerPitchReservationBLL { get; set; }
        public SoccerPitchReservationUoW(IEasySoccerDbContext dbContext, ISoccerPitchReservationBLL soccerPitchReservationBLL) : base(dbContext)
        {
            SoccerPitchReservationBLL = soccerPitchReservationBLL;
        }
    }
}
