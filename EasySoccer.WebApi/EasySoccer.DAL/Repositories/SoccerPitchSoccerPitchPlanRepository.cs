using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class SoccerPitchSoccerPitchPlanRepository : RepositoryBase, ISoccerPitchSoccerPitchPlanRepository
    {
        public SoccerPitchSoccerPitchPlanRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<SoccerPitchSoccerPitchPlan>> GetAsync(long soccerPitch)
        {
            return _dbContext.SoccerPitchSoccerPitchPlanQuery.Where(x => x.SoccerPitchId == soccerPitch).ToListAsync();
        }
    }
}
