using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class UserTokenRepository : RepositoryBase, IUserTokenRepository
    {
        public UserTokenRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<UserToken> GetAsync(string token, long userId)
        {
            return _dbContext.UserTokenQuery.Where(x => x.Token == token && x.CompanyUserId == userId).FirstOrDefaultAsync();
        }
    }
}
