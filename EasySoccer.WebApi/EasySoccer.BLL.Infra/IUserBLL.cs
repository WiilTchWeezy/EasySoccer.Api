using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IUserBLL
    {
        Task<User> LoginAsync(string email, string password);
        Task<List<User>> GetAsync(string filter);
        Task<User> CreateAsync(User user);
        Task<User> LoginFromFacebookAsync(string email, string id, string name, string birthday);
        Task<bool> ChangeUserPassword(string oldPassword, Guid userId, string newPassword);
        Task<User> GetAsync(Guid userId);
        Task<User> UpdateAsync(Guid userId, string name, string email, string phoneNumber);
    }
}
