using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IUserRepository : IRepositoryBase
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> LoginAsync(string socialMediaId);
        Task<List<User>> GetAsync(string filter);
    }
}
