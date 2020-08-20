using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IUserRepository : IRepositoryBase
    {
        Task<User> LoginBySocialMediaAsync(string socialMediaId);
        Task<User> GetAsync(Guid userId);
    }
}
