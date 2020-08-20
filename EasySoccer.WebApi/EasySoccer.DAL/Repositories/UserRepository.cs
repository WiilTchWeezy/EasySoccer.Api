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
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User> LoginBySocialMediaAsync(string socialMediaId)
        {
            return _dbContext.UserQuery.Where(x => x.SocialMediaId == socialMediaId).FirstOrDefaultAsync();
        }

        public Task<User> GetAsync(Guid userId)
        {
            return _dbContext.UserQuery.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }


    }
}
