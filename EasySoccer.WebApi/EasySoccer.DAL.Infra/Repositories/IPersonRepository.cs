using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IPersonRepository : IRepositoryBase
    {
        Task<List<Person>> GetAsync(string filter);
        Task<Person> GetByEmailAsync(string email);
        Task<Person> GetByPhoneAsync(string phone);
        Task<Person> GetByUserIdAsync(Guid userId);
        Task<Person> GetByPersonId(Guid personId);
    }
}
