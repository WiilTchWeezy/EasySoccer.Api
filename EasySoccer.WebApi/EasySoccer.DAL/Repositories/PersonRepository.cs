using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class PersonRepository : RepositoryBase, IRepositoryBase, IPersonRepository
    {
        public PersonRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Person>> GetAsync(string filter)
        {
            return _dbContext.PersonQuery.Where(x => x.Name.Contains(filter) || x.Phone.Contains(filter)).ToListAsync();
        }

        public Task<Person> GetAsync(string email, string phone)
        {
            var query = _dbContext.PersonQuery;
            query = query.Where(x => (email == null || x.Email == email) || (phone == null && x.Phone == phone));
            return query.FirstOrDefaultAsync();

        }

        public Task<Person> GetByEmailAsync(string email)
        {
            return _dbContext.PersonQuery.Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public Task<Person> GetByPersonId(Guid personId)
        {
            return _dbContext.PersonQuery.Where(x => x.Id == personId).FirstOrDefaultAsync();
        }

        public Task<Person> GetByPhoneAsync(string phone)
        {
            return _dbContext.PersonQuery.Where(x => x.Phone == phone).FirstOrDefaultAsync();
        }

        public Task<Person> GetByUserIdAsync(Guid userId)
        {
            return _dbContext.PersonQuery.Where(x => x.UserId != null && x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
