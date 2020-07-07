using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        public CompanyRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Company>> GetAsync(string description, int page, int pageSize)
        {
            return _dbContext.CompanyQuery.Where(x => (description == null || x.Description.Contains(description)) && x.Active == true).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<Company> GetAsync(long id)
        {
            return _dbContext.CompanyQuery.Include("CompanySchedules").Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<Company> GetAsync(string companyDocument)
        {
            return _dbContext.CompanyQuery.Where(x => x.CNPJ == companyDocument).FirstOrDefaultAsync();
        }
    }
}
