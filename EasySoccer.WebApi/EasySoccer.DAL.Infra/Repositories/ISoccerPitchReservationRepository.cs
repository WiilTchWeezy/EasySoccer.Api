using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchReservationRepository : IRepositoryBase
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long[] soccerPitchs, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(long[] soccerPitchs, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName);
        Task<SoccerPitchReservation> GetAsync(Guid id);
        Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName);
        Task<List<SoccerPitchReservation>> GetResumeAsync();
        Task<int> GetTotalByMonthAsync(int month);
        Task<List<SoccerPitchReservation>> GetAsync(int month, int day, long companyId);
        Task<List<SoccerPitchReservation>> GetAsync(int month, long companyId, int year);
        Task<SoccerPitchReservation> GetAsync(DateTime selectedDate, long companyId, long soccerPitchId);
        Task<List<SoccerPitchReservation>> GetByUserAsync(Guid userId);
        Task<SoccerPitchReservation> GetAsync(DateTime dateStart, DateTime dateEnd, long soccerPitch);
    }
}
