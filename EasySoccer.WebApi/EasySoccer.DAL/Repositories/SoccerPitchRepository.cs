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
    public class SoccerPitchRepository : RepositoryBase, ISoccerPitchRepository
    {
        public SoccerPitchRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<long[]> GetByCompanyIdAsync(long companyId)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == companyId).Select(x => x.Id).ToArrayAsync();
        }

        public Task<List<SoccerPitch>> GetAsync(int page, int pageSize, long companyId)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == companyId).Include(x => x.SoccerPitchSoccerPitchPlans).Include(x => x.SportType).Include("SoccerPitchSoccerPitchPlans.SoccerPitchPlan").Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<List<SoccerPitch>> GetByCompanyAsync(int company)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == company).OrderBy(x => x.Name).ToListAsync();
        }

        public Task<SoccerPitch> GetAsync(long id)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> GetTotalAsync()
        {
            return _dbContext.SoccerPitchQuery.CountAsync();
        }

        public Task<List<SoccerPitch>> GetAsync(long companyId, int sportType)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == companyId && x.SportTypeId == sportType).ToListAsync();
        }

        public Task<SoccerPitch> GetAsync(long companyId, long soccerPitchId)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == companyId && x.Id == soccerPitchId).FirstOrDefaultAsync();
        }
    }
}
