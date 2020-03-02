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

        public async Task<bool> ChangeUserPassword(string oldPassword, Guid userId, string newPassword)
        {
            var currentUser = await _userRepository.GetAsync(userId);
            if (currentUser == null)
                throw new BussinessException("Usuário não encontrado");

            if (!oldPassword.Equals(currentUser.Password))
                throw new BussinessException("A senha antiga não está correta");

            currentUser.Password = newPassword;
            await _userRepository.Edit(currentUser);
            await _dbContext.SaveChangesAsync();
            return true;
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
                user = await _userRepository.LoginBySocialMediaAsync(password, email);
            return user;
        }

        public async Task<User> LoginFromFacebookAsync(string email, string id, string name, string birthday)
        {
            var user = await _userRepository.LoginBySocialMediaAsync(id, email);
            if (user != null)
            {
                return user;
            }
            else
            {
                var createdUser = new User
                {
                    CreatedDate = DateTime.Now,
                    Email = email,
                    Id = Guid.NewGuid(),
                    Name = name,
                    Password = id,
                    SocialMediaId = id
                };
                await _userRepository.Create(createdUser);
                await _dbContext.SaveChangesAsync();
                return createdUser;
            }
        }

        public async Task<User> GetAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
                throw new BussinessException("Usuário não encontrado.");
            return user;
        }
    }
}
