using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchReservationRepository : IRepositoryBase
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long[] soccerPitchs, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(long[] soccerPitchs, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status);
        Task<SoccerPitchReservation> GetAsync(Guid id, bool includePerson = false, bool includeSoccerPitchInfo = false);
        Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status);
        Task<List<SoccerPitchReservation>> GetResumeAsync();
        Task<int> GetTotalByMonthAsync(int month);
        Task<List<SoccerPitchReservation>> GetAsync(int month, int day, long companyId, List<long> socerPitches, List<int> status);
        Task<List<SoccerPitchReservation>> GetAsync(int month, long companyId, int year, List<long> socerPitches, List<int> status);
        Task<SoccerPitchReservation> GetAsync(DateTime selectedDate, long companyId, long soccerPitchId);
        Task<List<SoccerPitchReservation>> GetByPersonCompanyAsync(Guid personCompanyId, int page, int pageSize);
        Task<SoccerPitchReservation> GetAsync(DateTime dateStart, DateTime dateEnd, long soccerPitch);
        Task<List<SoccerPitchReservation>> GetAsync(long companyId, DateTime selectedDate);
        Task<List<SoccerPitchReservation>> GetByOriginReservationAsync(Guid value);
    }
}
