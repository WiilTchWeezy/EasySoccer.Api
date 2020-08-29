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

        public Task<List<Company>> GetAsync(int page, int pageSize, string name, string orderField, string orderDirection)
        {
            var query = _dbContext.CompanyQuery.Where(x => x.Active == true).Skip((page - 1) * pageSize).Take(pageSize);
            if (string.IsNullOrEmpty(name) == false)
                query = query.Where(x => x.Name.Contains(name));
            if (orderField == "Name" && orderDirection == "ASC")
                query = query.OrderBy(x => x.Name);
            if (orderField == "Name" && orderDirection == "DESC")
                query = query.OrderByDescending(x => x.Name);
            return query.ToListAsync();
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
