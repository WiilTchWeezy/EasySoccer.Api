using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
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
                .Where(x => x.SelectedDateStart.Date >= date.Date && soccerPitchs.Contains(x.SoccerPitchId))
                .Include(x => x.SoccerPitch)
                .Include(x => x.PersonCompany)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(long[] soccerPitchs, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status)
        {
            return _dbContext.SoccerPitchReservationQuery
                .Where(
                x => soccerPitchs.Contains(x.SoccerPitchId)
                && (initialDate.HasValue == false || x.SelectedDateStart.Date >= initialDate.Value.Date)
                && (finalDate.HasValue == false || x.SelectedDateStart.Date <= finalDate.Value.Date)
                && (soccerPitchId.HasValue == false || x.SoccerPitchId == soccerPitchId)
                && (soccerPitchPlanId.HasValue == false || x.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId == soccerPitchPlanId)
                && (userName == null || (x.PersonCompany.Name.Contains(userName) || x.PersonCompany.Phone.Contains(userName)))
                && (status == null || status.Contains(x.Status))
                )
                .Include(x => x.SoccerPitch)
                .Include(x => x.PersonCompany)
                .Include(x => x.SoccerPitchSoccerPitchPlan)
                .Include(x => x.SoccerPitchSoccerPitchPlan.SoccerPitchPlan)
                .OrderByDescending(x => x.SelectedDateStart)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<SoccerPitchReservation> GetAsync(Guid id, bool includePerson = false, bool includeSoccerPitchInfo = false)
        {
            var query = _dbContext.SoccerPitchReservationQuery.Where(x => x.Id == id);
            if (includePerson)
            {
                query = query.Include(x => x.PersonCompany);
            }
            if (includeSoccerPitchInfo)
                query = query.Include(x => x.SoccerPitch).Include(x => x.SoccerPitch.Company).Include(x => x.SoccerPitch.Company.City).Include(x => x.SoccerPitchSoccerPitchPlan).Include(x => x.SoccerPitchSoccerPitchPlan.SoccerPitchPlan).Include(x => x.SoccerPitch.SportType);
            return query.FirstOrDefaultAsync();
        }

        public Task<SoccerPitchReservation> GetAsync(DateTime selectedDate, long companyId, long soccerPitchId)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch)
                             .Where(x => x.SelectedDateStart == selectedDate && x.SoccerPitch.CompanyId == companyId && x.SoccerPitchId == soccerPitchId).FirstOrDefaultAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(int month, int day, long companyId, List<long> soccerPitches)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.PersonCompany)
                .Where(x => 
                x.SelectedDateStart.Day == day && 
                x.SelectedDateStart.Month == month && 
                x.SelectedDateStart.Year == DateTime.Now.Year && 
                x.SoccerPitch.CompanyId == companyId && 
                x.Status != Entities.Enum.StatusEnum.Canceled && 
                (soccerPitches == null || soccerPitches.Count == 0 || soccerPitches.Contains(x.SoccerPitchId))).ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(int month, long companyId, int year, List<long> soccerPitches)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.PersonCompany)
                .Where(x =>
                x.SelectedDateStart.Month == month
                && x.SelectedDateStart.Year == year
                && x.SoccerPitch.CompanyId == companyId
                && x.Status != Entities.Enum.StatusEnum.Canceled &&
                (soccerPitches == null || soccerPitches.Count == 0 || soccerPitches.Contains(x.SoccerPitchId))).ToListAsync();
        }

        public Task<SoccerPitchReservation> GetAsync(DateTime dateStart, DateTime dateEnd, long soccerPitch)
        {
            return _dbContext.SoccerPitchReservationQuery.Where(x =>
            x.SoccerPitchId == soccerPitch &&
            x.Status == StatusEnum.Confirmed &&
            (
            (x.SelectedDateStart >= dateStart && x.SelectedDateStart <= dateEnd)
            ||
            (x.SelectedDateEnd > dateStart && x.SelectedDateEnd <= dateEnd))
            ).FirstOrDefaultAsync();
        }

        public Task<List<SoccerPitchReservation>> GetByPersonCompanyAsync(Guid personCompanyId, int page, int pageSize)
        {
            return _dbContext.SoccerPitchReservationQuery
                .Where(x => x.PersonCompanyId != null && x.PersonCompanyId == personCompanyId)
                .Include(x => x.PersonCompany).Include(x => x.SoccerPitch).Include(x => x.SoccerPitch.Company)
                .OrderByDescending(x => x.SelectedDateStart)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<List<SoccerPitchReservation>> GetResumeAsync()
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.PersonCompany).Take(10).ToListAsync();
        }

        public Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.PersonCompany)
                .Where(
                x => x.SoccerPitch.CompanyId == companyId
                && (initialDate.HasValue == false || x.SelectedDateStart.Date >= initialDate.Value.Date)
                && (finalDate.HasValue == false || x.SelectedDateStart.Date <= finalDate.Value.Date)
                && (soccerPitchId.HasValue == false || x.SoccerPitchId == soccerPitchId)
                && (soccerPitchPlanId.HasValue == false || x.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId == soccerPitchPlanId)
                && (userName == null || (x.PersonCompany.Name.Contains(userName) || x.PersonCompany.Phone.Contains(userName)))
                && (status == null || status.Contains(x.Status))
                ).CountAsync();
        }

        public Task<int> GetTotalByMonthAsync(int month)
        {
            return _dbContext.SoccerPitchReservationQuery.Where(x => x.SelectedDateStart.Month == month).CountAsync();
        }

        public Task<List<SoccerPitchReservation>> GetAsync(long companyId, DateTime selectedDate)
        {
            return _dbContext.SoccerPitchReservationQuery.Include(x => x.SoccerPitch).Include(x => x.PersonCompany)
                .Where(x => x.SoccerPitch.CompanyId == companyId && x.SelectedDateStart.Date == selectedDate.Date).OrderBy(x => x.SoccerPitch.Name).ToListAsync();
        }
    }
}