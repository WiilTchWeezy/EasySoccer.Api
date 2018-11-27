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
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        public CompanyRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Company>> GetAsync(double? longitude, double? latitude, string description)
        {
            return _dbContext.CompanyQuery.Where(x => x.Description.Contains(description)).ToListAsync();
        }
    }
}
