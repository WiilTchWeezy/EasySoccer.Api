using EasySoccer.BLL.Enums;
using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class SoccerPitchReservationBLL : ISoccerPitchReservationBLL
    {
        private ISoccerPitchReservationRepository _soccerPitchReservationRepository;
        private ISoccerPitchRepository _soccerPitchRepository;
        private IEasySoccerDbContext _dbContext;
        private ISoccerPitchSoccerPitchPlanRepository _soccerPitchSoccerPitchPlanRepository;
        public SoccerPitchReservationBLL
            (ISoccerPitchReservationRepository soccerPitchReservationRepository,
            ISoccerPitchRepository soccerPitchRepository,
            IEasySoccerDbContext dbContext,
            ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
        }

        public async Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long companyUserId, long soccerPitchPlanId)
        {
            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchPlanId);
            if (soccerPicthPlanRelation == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);

            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDate = selectedDate,
                SelectedHourEnd = hourFinish,
                SelectedHourStart = hourStart,
                SoccerPitchId = soccerPitchId,
                Status = (int)StatusEnum.AguardandoAprovacao,
                StatusChangedUserId = companyUserId,
                UserId = userId,
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id
            };
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchPlanId)
        {
            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchPlanId);
            if (soccerPicthPlanRelation == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);

            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDate = selectedDate,
                SelectedHourEnd = hourFinish,
                SelectedHourStart = hourStart,
                SoccerPitchId = soccerPitchId,
                Status = (int)StatusEnum.AguardandoAprovacao,
                StatusChangedUserId = null,
                UserId = userId,
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id
            };
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(date, companyPitchs, page, pageSize);
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(long companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(companyPitchs, page, pageSize);
        }

        public async Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime seledtedDate, int sportType)
        {
            List<AvaliableSchedulesDTO> avaliableSchedules = new List<AvaliableSchedulesDTO>();
            var soccerPitchsBySportType = await _soccerPitchRepository.GetAsync(companyId, sportType);
            AvaliableSchedulesDTO userSelectedSchedule = null;
            foreach (var item in soccerPitchsBySportType)
            {
                var selectedSchedule = await _soccerPitchReservationRepository.GetAsync(seledtedDate, companyId, item.Id);
                if (selectedSchedule == null)
                {
                    if (userSelectedSchedule == null)
                    {
                        userSelectedSchedule = new AvaliableSchedulesDTO
                        {
                            IsCurrentSchedule = true,
                            SelectedDate = seledtedDate,
                            SelectedHourStart = seledtedDate.TimeOfDay,
                            SelectedHourEnd = seledtedDate.AddHours(1).TimeOfDay,
                            PossibleSoccerPitchs = new List<SoccerPitch>()
                        };
                    }
                    userSelectedSchedule.PossibleSoccerPitchs.Add(item);
                }
            }
            if (userSelectedSchedule != null)
                avaliableSchedules.Add(userSelectedSchedule);
            return avaliableSchedules;
        }

        public async Task<List<ReservationChart>> GetReservationChartDataAsync(DateTime startDate)
        {
            var reservationChart = new List<ReservationChart>();
            for (int i = 0; i > -6; i--)
            {
                var selectedDate = startDate.AddMonths(i);
                var reservation = await _soccerPitchReservationRepository.GetTotalByMonthAsync(selectedDate.Month);
                reservationChart.Add(new ReservationChart
                {
                    DataCount = reservation,
                    DataLabel = selectedDate.ToString("MMM")
                });
            }
            return reservationChart;
        }

        public Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day)
        {
            if (day.HasValue && day.Value > 0)
            {
                return _soccerPitchReservationRepository.GetAsync(month, day.Value);
            }
            else
            {
                return _soccerPitchReservationRepository.GetAsync(month);
            }
        }

        public Task<List<SoccerPitchReservation>> GetResumeAsync()
        {
            return _soccerPitchReservationRepository.GetResumeAsync();
        }

        public Task<int> GetTotalAsync()
        {
            return _soccerPitchReservationRepository.GetTotalAsync();
        }

        public async Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId)
        {
            var soccerPitchReservation = await _soccerPitchReservationRepository.GetAsync(id);
            if (soccerPitchReservation == null)
                throw new NotFoundException(soccerPitchReservation, id);

            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchSoccerPitchPlanId);
            if (soccerPicthPlanRelation != null)
                soccerPitchReservation.SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id;

            soccerPitchReservation.Note = note;
            soccerPitchReservation.SelectedDate = selectedDate;
            soccerPitchReservation.SelectedHourEnd = hourFinish;
            soccerPitchReservation.SelectedHourStart = hourStart;
            soccerPitchReservation.SoccerPitchId = soccerPitchId;
            soccerPitchReservation.UserId = userId;
            await _soccerPitchReservationRepository.Edit(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }
    }
}
