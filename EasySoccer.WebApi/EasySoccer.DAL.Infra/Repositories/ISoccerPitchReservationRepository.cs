using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchReservationRepository : IRepositoryBase
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long[] soccerPitchs, int page, int pageSize);
        Task<List<SoccerPitchReservation>> GetAsync(long[] soccerPitchs, int page, int pageSize);
        Task<SoccerPitchReservation> GetAsync(Guid id);
        Task<int> GetTotalAsync();
        Task<List<SoccerPitchReservation>> GetResumeAsync();
    }
}
