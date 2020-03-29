using EasySoccer.Entities;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyScheduleRepository : IRepositoryBase
    {
        Task<CompanySchedule> GetAsync(long Id);
        Task<CompanySchedule> GetAsync(long companyId, int dayOfWeek);

    }
}
