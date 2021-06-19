using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class PersonCompanyRepository : RepositoryBase, IPersonCompanyRepository
    {
        public PersonCompanyRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<PersonCompany> GetAsync(Guid id)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<PersonCompany>> GetAsync(string filter, long companyId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId && (x.Name.Contains(filter) || x.Phone.Contains(filter))).ToListAsync();
        }

        public Task<List<PersonCompany>> GetAsync(long companyId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        public Task<PersonCompany> GetAsync(string email, string phone, long companyId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId && (x.Email == email || x.Phone == phone)).FirstOrDefaultAsync();
        }

        public Task<List<PersonCompany>> GetAsync(string name, string email, string phone, int page, int pageSize, long companyId)
        {
            var query = _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId);
            if (string.IsNullOrWhiteSpace(phone) == false)
                query = query.Where(x => x.Phone.Contains(phone));
            if (string.IsNullOrWhiteSpace(email) == false)
                query = query.Where(x => x.Email.Contains(email));
            if (string.IsNullOrWhiteSpace(name) == false)
                query = query.Where(x => x.Name.Contains(name));

            return query.Skip((page - 1) * pageSize).Take(pageSize).OrderBy(x => x.Name).ToListAsync();
        }

        public Task<int> GetAsync(string name, string email, string phone, long companyId)
        {
            var query = _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId);
            if (string.IsNullOrWhiteSpace(phone) == false)
                query = query.Where(x => x.Phone.Contains(phone));
            if (string.IsNullOrWhiteSpace(email) == false)
                query = query.Where(x => x.Email.Contains(email));
            if (string.IsNullOrWhiteSpace(name) == false)
                query = query.Where(x => x.Name.Contains(name));

            return query.CountAsync();
        }

        public Task<PersonCompany> GetByEmailAsync(string email, long companyId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.Email == email && x.CompanyId == companyId).FirstOrDefaultAsync();
        }

        public Task<PersonCompany> GetByPersonIdAsync(Guid personId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.PersonId == personId).FirstOrDefaultAsync();
        }

        public Task<PersonCompany> GetByPhoneAsync(string phone, long companyId)
        {
            return _dbContext.PersonCompanyQuery.Where(x => x.CompanyId == companyId && x.Phone == phone).FirstOrDefaultAsync();
        }
    }
}
