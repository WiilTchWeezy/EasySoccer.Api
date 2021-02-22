using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchReservationBLL
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long companyId, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(long companyId, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum? status);
        Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long? companyUserId, long soccerPitchPlanId, ApplicationEnum application);
        Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId);
        Task<List<SoccerPitchReservation>> GetResumeAsync();
        Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum? status);
        Task<List<ReservationChart>> GetReservationChartDataAsync(DateTime startDate);
        Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day, long companyId, int year);
        Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime seledtedDate, int sportType);
        Task<List<SoccerPitchReservation>> GetUserSchedulesAsync(Guid userId, int page, int pageSize);
        Task<List<GetSchedulesResponse>> GetSchedulesResponses(long companyId, DateTime selectDate);
        Task<SoccerPitchReservation> GetReservationInfoAsync(Guid reservationId);
        Task<bool> ChangeStatusAsync(Guid reservationId, StatusEnum status, long userId);
    }
}
