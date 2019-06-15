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

        public Task<long[]> GetAsync(int companyId)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == companyId).Select(x => x.Id).ToArrayAsync();
        }

        public Task<List<SoccerPitch>> GetAsync(int page, int pageSize)
        {
            return _dbContext.SoccerPitchQuery.Include(x => x.SoccerPitchSoccerPitchPlans).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<List<SoccerPitch>> GetByCompanyAsync(int company)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.CompanyId == company).ToListAsync();
        }

        public Task<SoccerPitch> GetAsync(long id)
        {
            return _dbContext.SoccerPitchQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
