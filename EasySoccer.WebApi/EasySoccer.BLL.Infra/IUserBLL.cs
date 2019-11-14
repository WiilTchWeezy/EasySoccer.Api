using EasySoccer.Entities;
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
    }
}
