using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
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
        private IEasySoccerDbContext _dbContext;
        public UserBLL(IUserRepository userRepository, IEasySoccerDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Phone) == false)
            {
                var currentUser = await _userRepository.GetByPhoneAsync(user.Phone);
                if (currentUser != null)
                    throw new BussinessException("Usuário já cadastrado com este telefone.");
            }
            if (string.IsNullOrEmpty(user.Email) == false)
            {
                var currentUser = await _userRepository.GetByEmailAsync(user.Email);
                if (currentUser != null)
                    throw new BussinessException("Usuário já cadastrado com este email.");
            }
            user.CreatedDate = DateTime.Now;
            await _userRepository.Create(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public Task<List<User>> GetAsync(string filter)
        {
            return _userRepository.GetAsync(filter);
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
