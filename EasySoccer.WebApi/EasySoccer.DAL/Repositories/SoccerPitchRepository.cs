using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
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
    }
}
