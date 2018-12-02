using EasySoccer.Entities;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IUserBLL
    {
        Task<User> LoginAsync(string email, string password);
    }
}
