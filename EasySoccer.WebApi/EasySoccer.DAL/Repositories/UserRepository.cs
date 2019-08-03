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

        public Task<List<User>> GetAsync(string filter)
        {
            return _dbContext.UserQuery.Where(x => x.Name.Contains(filter) || x.Phone.Contains(filter)).ToListAsync();
        }

        public Task<User> GetByPhoneAsync(string phone)
        {
            return _dbContext.UserQuery.Where(x => x.Phone == phone).FirstOrDefaultAsync();
        }

        public Task<User> LoginAsync(string email, string password)
        {
            return _dbContext.UserQuery.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public Task<User> LoginAsync(string socialMediaId)
        {
            return _dbContext.UserQuery.Where(x => x.SocialMediaId == socialMediaId).FirstOrDefaultAsync();
        }
    }
}
