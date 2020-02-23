using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IUserRepository : IRepositoryBase
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> LoginBySocialMediaAsync(string socialMediaId, string email);
        Task<List<User>> GetAsync(string filter);
        Task<User> GetByPhoneAsync(string phone);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetAsync(Guid userId);
    }
}
