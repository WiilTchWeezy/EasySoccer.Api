using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.BLL.Infra.Services.Cryptography;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class UserBLL : IUserBLL
    {
        private IUserRepository _userRepository;
        private IEasySoccerDbContext _dbContext;
        private IPersonRepository _personRepository;
        private ICryptographyService _cryptographyService;
        public UserBLL(IUserRepository userRepository, IPersonRepository personRepository, IEasySoccerDbContext dbContext, ICryptographyService cryptographyService)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _personRepository = personRepository;
            _cryptographyService = cryptographyService;
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

        public async Task<List<PersonUserResponse>> GetAsync(string filter)
        {
            var persons = await _personRepository.GetAsync(filter);
            return persons.Select(x => new PersonUserResponse
            {
                Email = x.Email,
                Name = x.Name,
                PersonId = x.Id,
                Phone = x.Phone
            }).ToList();

        }

        public async Task<PersonUserResponse> LoginAsync(string email, string password)
        {
            PersonUserResponse personUserResponse = null;
            var person = await _personRepository.GetByEmailAsync(email);
            if (person != null && person.UserId.HasValue)
            {
                var user = await _userRepository.GetAsync(person.UserId.Value);
                if (user != null)
                {
                    if (password.Equals(user.Password))
                        personUserResponse = new PersonUserResponse(person, user);
                }
            }
            return personUserResponse;

        }

        public async Task<PersonUserResponse> LoginFromFacebookAsync(string email, string id, string name, string birthday)
        {
            var user = await _userRepository.LoginBySocialMediaAsync(id);
            if (user != null)
            {
                var person = await _personRepository.GetAsync(user.Id);
                if (person == null)
                    throw new BussinessException("Ops! Ocorreu um erro, cadastro não encontrado.");
                var personUser = new PersonUserResponse(person, user);
                return personUser;
            }
            else
            {
                var person = await _personRepository.GetByEmailAsync(email);
                if (person != null)
                {
                    if (person.UserId.HasValue)
                        throw new BussinessException("Ops! Ocorreu um erro, email já cadastrado.");
                    person.Name = name;
                    person.Phone = string.Empty;
                    var userToAdd = new User
                    {
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        SocialMediaId = id,
                        CreatedFrom = CreatedFromEnum.Mobile,
                        Password = String.Empty
                    };
                    person.UserId = userToAdd.Id;
                    await _userRepository.Create(userToAdd);
                    await _personRepository.Edit(person);
                    await _dbContext.SaveChangesAsync();
                    return new PersonUserResponse(person, userToAdd);
                }
                else
                {
                    var userToAdd = new User
                    {
                        CreatedDate = DateTime.Now,
                        CreatedFrom = CreatedFromEnum.Mobile,
                        Id = Guid.NewGuid(),
                        Password = string.Empty,
                        SocialMediaId = id
                    };
                    var personToAdd = new Person
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        CreatedFrom = CreatedFromEnum.Mobile,
                        Email = email,
                        Name = name,
                        Phone = string.Empty,
                        UserId = userToAdd.Id
                    };
                    await _userRepository.Create(userToAdd);
                    await _personRepository.Create(personToAdd);
                    await _dbContext.SaveChangesAsync();
                    return new PersonUserResponse(personToAdd, userToAdd);
                }
            }
        }

        public async Task<PersonUserResponse> GetAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
                throw new BussinessException("Usuário não encontrado.");
            var person = await _personRepository.GetAsync(userId);
            if (person == null)
                throw new BussinessException("Pessoa não encontrado.");
            var personUserResponse = new PersonUserResponse(person, user);
            return personUserResponse;
        }

        public async Task<PersonUserResponse> UpdateAsync(Guid userId, string name, string email, string phoneNumber)
        {
            var person = await _personRepository.GetAsync(userId);
            if (person == null)
                throw new BussinessException("Usuário não encontrado");
            if (!string.IsNullOrEmpty(name))
                person.Name = name;
            if (!string.IsNullOrEmpty(email))
                person.Email = email;
            if (!string.IsNullOrEmpty(phoneNumber))
                person.Phone = phoneNumber;
            await _personRepository.Edit(person);
            await _dbContext.SaveChangesAsync();
            return new PersonUserResponse(person);
        }

        public async Task<PersonUserResponse> CreatePersonAsync(string name, string phone, string email, CreatedFromEnum createdFrom)
        {
            if (string.IsNullOrEmpty(phone))
                throw new BussinessException("É necessário preencher um telefone.");

            if (string.IsNullOrEmpty(name))
                throw new BussinessException("É necessário preencher um nome.");

            var person = new Person
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedFrom = createdFrom,
                Email = email,
                Name = name,
                Phone = phone
            };
            await _personRepository.Create(person);
            await _dbContext.SaveChangesAsync();
            var personResponse = new PersonUserResponse
            {
                Email = person.Email,
                Name = person.Name,
                PersonId = person.Id,
                Phone = person.Phone
            };
            return personResponse;
        }

        public async Task<PersonUserResponse> CreateUserAsync(string name, string phoneNumber, string email, string password)
        {
            var personByEmail = await _personRepository.GetByEmailAsync(email);
            if (personByEmail != null)
            {
                if (personByEmail.UserId.HasValue)
                    throw new BussinessException("Existe um usuário cadastrado com este email!");
                if (!phoneNumber.Equals(personByEmail.Phone))
                    personByEmail.Phone = phoneNumber;
                personByEmail.Name = name;
                var user = new User
                {
                    CreatedDate = DateTime.Now,
                    CreatedFrom = CreatedFromEnum.Mobile,
                    Id = Guid.NewGuid(),
                    Password = _cryptographyService.Encrypt(password)
                };
                personByEmail.UserId = user.Id;
                await _userRepository.Create(user);
                await _personRepository.Edit(personByEmail);
                await _dbContext.SaveChangesAsync();
                var response = new PersonUserResponse(personByEmail, user);
                response.FoundedPersonByEmail = true;
                response.FoundedPersonByPhone = true;
                return response;
            }
            else
            {
                var personByPhone = await _personRepository.GetByPhoneAsync(phoneNumber);
                if(personByPhone != null)
                {
                    if (personByPhone.UserId.HasValue)
                        throw new BussinessException("Existe um usuário cadastrado com este telefone!");
                    if (!phoneNumber.Equals(personByPhone.Phone))
                        personByPhone.Phone = phoneNumber;
                    personByPhone.Name = name;
                    var user = new User
                    {
                        CreatedDate = DateTime.Now,
                        CreatedFrom = CreatedFromEnum.Mobile,
                        Id = Guid.NewGuid(),
                        Password = _cryptographyService.Encrypt(password)
                    };
                    personByPhone.UserId = user.Id;
                    await _userRepository.Create(user);
                    await _personRepository.Edit(personByPhone);
                    await _dbContext.SaveChangesAsync();
                    var response = new PersonUserResponse(personByPhone, user);
                    response.FoundedPersonByEmail = true;
                    response.FoundedPersonByPhone = true;
                    return response;
                }
                else
                {
                    var user = new User
                    {
                        CreatedDate = DateTime.Now,
                        CreatedFrom = CreatedFromEnum.Mobile,
                        Id = Guid.NewGuid(),
                        Password = _cryptographyService.Encrypt(password)
                    };
                    var person = new Person
                    {
                        Id = Guid.NewGuid(),
                        CreatedFrom = CreatedFromEnum.Mobile,
                        CreatedDate = DateTime.Now,
                        Email = email,
                        Name = name,
                        Phone = phoneNumber,
                        UserId = user.Id
                    };
                    await _userRepository.Create(user);
                    await _personRepository.Create(person);
                    await _dbContext.SaveChangesAsync();
                    var response = new PersonUserResponse(person, user);
                    response.FoundedPersonByEmail = true;
                    response.FoundedPersonByPhone = true;
                    return response;
                }
            }
        }
    }
}
