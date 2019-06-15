using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchSoccerPitchPlanRepository : IRepositoryBase
    {
        Task<List<SoccerPitchSoccerPitchPlan>> GetAsync(long soccerPitch);
    }
}
