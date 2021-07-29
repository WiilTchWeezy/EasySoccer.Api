using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchRepository : IRepositoryBase
    {
        Task<long[]> GetByCompanyIdAsync(long companyId);
        Task<List<SoccerPitch>> GetAsync(int page, int pageSize, long companyId);
        Task<SoccerPitch> GetAsync(long id, bool includeInfo = false);
        Task<List<SoccerPitch>> GetByCompanyAsync(int company);
        Task<int> GetTotalAsync(long companyId);
        Task<List<SoccerPitch>> GetAsync(long companyId, int sportType);
        Task<SoccerPitch> GetAsync(long companyId, long soccerPitchId);
    }
}
