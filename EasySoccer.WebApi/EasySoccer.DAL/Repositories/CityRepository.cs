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
    public class CityRepository : RepositoryBase, ICityRepository
    {
        public CityRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<City> GetAsync(int id)
        {
            return _dbContext.CityQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
