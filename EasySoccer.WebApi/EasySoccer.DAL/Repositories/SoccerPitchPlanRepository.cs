using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class SoccerPitchPlanRepository : RepositoryBase , ISoccerPitchPlanRepository
    {
        public SoccerPitchPlanRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<SoccerPitchPlan>> GetAsync(long companyId, int page, int pageSize)
        {
            return _dbContext.SoccerPitchPlanQuery
                .Where(x => x.CompanyId == companyId)
                .Skip((page -1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public Task<SoccerPitchPlan> GetAsync(int id)
        {
            return _dbContext.SoccerPitchPlanQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
