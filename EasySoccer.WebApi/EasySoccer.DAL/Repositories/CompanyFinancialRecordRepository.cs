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
    public class CompanyFinancialRecordRepository : RepositoryBase, ICompanyFinancialRecordRepository
    {
        public CompanyFinancialRecordRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<CompanyFinancialRecord> GetByCompanyAsync(long companyId)
        {
            return _dbContext.CompanyFinancialRecordQuery.Where(x => x.CompanyId == companyId && x.Paid == true && x.ExpiresDate.Date >= DateTime.UtcNow.Date).FirstOrDefaultAsync();
        }

        public Task<List<CompanyFinancialRecord>> GetCompaniesToGetLate()
        {
            var date = DateTime.UtcNow.AddDays(-7);
            return _dbContext.CompanyFinancialRecordQuery.Include(x => x.Company).Where(x => x.Paid == true && x.ExpiresDate.Date >= date.Date).ToListAsync();
        }
    }
}
