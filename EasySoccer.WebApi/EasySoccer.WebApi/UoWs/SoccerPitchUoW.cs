using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class SoccerPitchUoW : UoWBase
    {
        public ISoccerPitchBLL SoccerPitchBLL { get; set; }
        public SoccerPitchUoW(IEasySoccerDbContext dbContext, ISoccerPitchBLL soccerPitchBLL) : base(dbContext)
        {
            SoccerPitchBLL = soccerPitchBLL;
        }
    }
}
