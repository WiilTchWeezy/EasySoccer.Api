using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchReservationBLL
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long companyId, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(long companyId, int page, int pageSize);
        Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long companyUserId, long soccerPitchPlanId);
        Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchPlanId);
        Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId);
        Task<List<SoccerPitchReservation>> GetResumeAsync();
        Task<int> GetTotalAsync();
        Task<List<ReservationChart>> GetReservationChartDataAsync(DateTime startDate);
        Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day, long companyId);
        Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime seledtedDate, int sportType);
        Task<List<SoccerPitchReservation>> GetUserSchedulesAsync(Guid userId);
    }
}
