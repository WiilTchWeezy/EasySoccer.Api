using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class UserBLL : IUserBLL
    {
        private IUserRepository _userRepository;
        public UserBLL(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> LoginAsync(string email, string password)
        {
            User user;
            user = await _userRepository.LoginAsync(email, password);
            if (user == null)
                user = await _userRepository.LoginAsync(password);
            return user;
        }
    }
}
