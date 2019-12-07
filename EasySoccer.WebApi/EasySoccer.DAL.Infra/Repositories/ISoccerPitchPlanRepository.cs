using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchPlanRepository: IRepositoryBase
    {
        Task<List<SoccerPitchPlan>> GetAsync(long companyId, int page, int pageSize);
        Task<SoccerPitchPlan> GetAsync(int id);
    }
}
