﻿using EasySoccer.DAL.Infra;
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
    public class SportTypeRepository : RepositoryBase, ISportTypeRepository
    {
        public SportTypeRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<SportType>> GetAsync()
        {
            return _dbContext.SportTypeQuery.ToListAsync();
        }

        public Task<List<SportType>> GetAsync(long companyId)
        {
            return _dbContext.SoccerPitchQuery
                            .Include(x => x.SportType)
                            .Where(x => x.CompanyId == companyId)
                            .Select(x => x.SportType).Distinct().ToListAsync();
        }
    }
}
