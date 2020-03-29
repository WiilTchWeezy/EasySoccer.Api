using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class CompanyScheduleRepository : RepositoryBase, ICompanyScheduleRepository
    {
        public CompanyScheduleRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<CompanySchedule> GetAsync(long Id)
        {
            return _dbContext.CompanyScheduleQuery.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public Task<CompanySchedule> GetAsync(long companyId, int dayOfWeek)
        {
            return _dbContext.CompanyScheduleQuery.Where(x => x.CompanyId == companyId && x.Day == dayOfWeek).FirstOrDefaultAsync();
        }
    }
}
