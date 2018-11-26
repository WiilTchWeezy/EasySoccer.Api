using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.DAL.Repositories
{
    public class SoccerPitchRepository : RepositoryBase, ISoccerPitchRepository
    {
        public SoccerPitchRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

    }
}
