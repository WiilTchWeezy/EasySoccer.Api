using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchReservationBLL
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, int companyId, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(int companyId, int page, int pageSize);
        Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long companyUserId, long selectedSoccerPitchPlaId);
        Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId);
        Task<List<SoccerPitchReservation>> GetResumeAsync();
        Task<int> GetTotalAsync();
    }
}
