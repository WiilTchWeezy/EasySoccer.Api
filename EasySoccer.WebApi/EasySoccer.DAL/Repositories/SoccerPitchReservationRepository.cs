using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class SoccerPitchReservationRepository : RepositoryBase, ISoccerPitchReservationRepository
    {
        public SoccerPitchReservationRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long[] soccerPitchs, int page, int pageSize)
        {
            return _dbContext.SoccerPitchReservationQuery
                .Where(x => x.SelectedDate.Date >= date.Date && soccerPitchs.Contains(x.SoccerPitchId))
                .Include(x => x.SoccerPitch)
                .Include(x => x.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(long[] soccerPitchs, int page, int pageSize)
        {
            return _dbContext.SoccerPitchReservationQuery
                .Where(x => soccerPitchs.Contains(x.SoccerPitchId))
                .Include(x => x.SoccerPitch)
                .Include(x => x.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<SoccerPitchReservation> GetAsync(Guid id)
        {
            return _dbContext.SoccerPitchReservationQuery.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<SoccerPitchReservation> GetAsync(DateTime selectedDate, long companyId, long soccerPitchId)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch)
                             .Where(x => x.SelectedDate.Date == selectedDate.Date && selectedDate.TimeOfDay == x.SelectedHourStart && x.SoccerPitch.CompanyId == companyId && x.SoccerPitchId == soccerPitchId).FirstOrDefaultAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(int month, int day)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.User)
                .Where(x => x.SelectedDate.Day == day && x.SelectedDate.Month == month && x.SelectedDate.Year == DateTime.Now.Year).ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(int month)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.User)
                .Where(x => x.SelectedDate.Month == month && x.SelectedDate.Year == DateTime.Now.Year).ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetResumeAsync()
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.User).Take(10).ToListAsync();
        }

        public Task<int> GetTotalAsync()
        {
            return _dbContext.SoccerPitchReservationQuery.CountAsync();
        }

        public Task<int> GetTotalByMonthAsync(int month)
        {
            return _dbContext.SoccerPitchReservationQuery.Where(x => x.SelectedDate.Month == month).CountAsync();
        }
    }
}