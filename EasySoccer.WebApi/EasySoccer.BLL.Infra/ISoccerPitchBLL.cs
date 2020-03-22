using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchBLL
    {
        Task<List<SoccerPitch>> GetAsync(int page, int pageSize, long companyId);
        Task<SoccerPitch> CreateAsync(string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int[] soccerPitchPlansId, int sportTypeId, int interval);
        Task<SoccerPitch> UpdateAsync(long id, string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int[] soccerPitchPlansId, int sportTypeId, int interval);
        Task<List<SportType>> GetSportTypeAsync();
        Task<int> GetTotalAsync();
    }
}
