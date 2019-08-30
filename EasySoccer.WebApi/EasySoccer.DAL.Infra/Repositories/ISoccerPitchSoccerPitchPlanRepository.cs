using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchSoccerPitchPlanRepository : IRepositoryBase
    {
        Task<List<SoccerPitchSoccerPitchPlan>> GetAsync(long soccerPitch);
        Task<SoccerPitchSoccerPitchPlan> GetAsync(long soccerPitch, long soccerPitchPlan);
        Task<List<SoccerPitchPlan>> GetPlansAsync(long soccerPitch);
    }
}
