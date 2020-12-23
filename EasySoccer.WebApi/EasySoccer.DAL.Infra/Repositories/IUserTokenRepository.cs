using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IUserTokenRepository : IRepositoryBase
    {
        Task<UserToken> GetAsync(string token, long userId);
        Task<UserToken> GetAsync(string token, Guid userId);
        Task<List<UserToken>> GetAsync(long[] companyUserIds);
        Task<List<UserToken>> GetAsync(Guid[] userIds);
    }
}
