using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchBLL
    {
        Task<List<SoccerPitch>> GetAsync(int page, int pageSize, long companyId);
        Task<SoccerPitch> CreateAsync(string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, SoccerPitchPlanRequest[] soccerPitchPlansId, int sportTypeId, int interval, string color, string imageBase64);
        Task<SoccerPitch> UpdateAsync(long id, string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, SoccerPitchPlanRequest[] soccerPitchPlansId, int sportTypeId, int interval, string color);
        Task<List<SportType>> GetSportTypeAsync();
        Task<List<SportType>> GetSportTypeAsync(long companyId);
        Task<int> GetTotalAsync();
        Task SaveImageAsync(long companyId, long soccerPitchId, string imageBase64);

        Task<SoccerPitch> GetByIdAsync(long id);

    }
}
