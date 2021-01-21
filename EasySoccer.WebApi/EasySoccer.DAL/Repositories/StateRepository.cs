using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class StateRepository : RepositoryBase, IStateRepository
    {
        public StateRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<State>> GetAsync()
        {
            return _dbContext.StateQuery.ToListAsync();
        }

        public Task<State> GetAsync(int id)
        {
            return _dbContext.StateQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
