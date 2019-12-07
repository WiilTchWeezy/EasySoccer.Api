using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchRepository : IRepositoryBase
    {
        Task<long[]> GetByCompanyIdAsync(long companyId);
        Task<List<SoccerPitch>> GetAsync(int page, int pageSize);
        Task<SoccerPitch> GetAsync(long id);
        Task<List<SoccerPitch>> GetByCompanyAsync(int company);
        Task<int> GetTotalAsync();
    }
}
