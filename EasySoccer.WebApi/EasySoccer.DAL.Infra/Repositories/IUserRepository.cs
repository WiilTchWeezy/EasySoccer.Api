using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IUserRepository
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> LoginAsync(string socialMediaId);
    }
}
