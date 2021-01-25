using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var query = _dbContext.UserTokenQuery.Where(x => x.Token == token && x.CompanyUserId != null && x.CompanyUserId.Value == userId);
            return query.FirstOrDefaultAsync();
        }

        public Task<List<UserToken>> GetAsync(long[] companyUserIds)
        {
            return _dbContext.UserTokenQuery.Where(x => x.CompanyUserId != null && companyUserIds.Contains(x.CompanyUserId.Value)).ToListAsync();
        }

        public Task<UserToken> GetAsync(string token, Guid userId)
        {
            var query = _dbContext.UserTokenQuery.Where(x => x.Token == token && x.UserId != null && x.UserId.Value == userId);
            return query.FirstOrDefaultAsync();
        }

        public Task<List<UserToken>> GetAsync(Guid[] userIds)
        {
            return _dbContext.UserTokenQuery.Where(x => x.UserId != null && userIds.Contains(x.UserId.Value)).ToListAsync();

        }

        public Task<List<UserToken>> GetAsync(long companyUserId)
        {
            return _dbContext.UserTokenQuery.Where(x => x.CompanyUserId != null && x.CompanyUserId.Value == userId && x.LogOffDate == null && x.IsActive == true).ToListAsync();
        }
    }
}
