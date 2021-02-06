using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchPlanBLL
    {
        Task<List<SoccerPitchPlan>> GetAsync(long companyId, int page, int pageSize);
        Task<List<SoccerPitchSoccerPitchPlan>> GetAsync(long soccerPitchId);
        Task<SoccerPitchPlan> CreateAsync(string name, decimal value, long companyId, string description);
        Task<SoccerPitchPlan> UpdateAsync(int id, string name, decimal value, string description);
    }
}
