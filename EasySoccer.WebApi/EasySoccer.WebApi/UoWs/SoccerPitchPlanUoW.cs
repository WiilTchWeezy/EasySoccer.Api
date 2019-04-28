using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class SoccerPitchPlanUoW : UoWBase
    {

        public ISoccerPitchPlanBLL SoccerPitchPlanBLL { get; set; }
        public SoccerPitchPlanUoW(IEasySoccerDbContext dbContext, ISoccerPitchPlanBLL soccerPitchPlanBLL) : base(dbContext)
        {
            SoccerPitchPlanBLL = soccerPitchPlanBLL;
        }
    }
}
