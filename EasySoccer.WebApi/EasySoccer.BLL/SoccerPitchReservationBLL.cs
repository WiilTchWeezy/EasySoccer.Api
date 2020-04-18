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
        private ICompanyScheduleRepository _companyScheduleRepository;
        public SoccerPitchReservationBLL
            (ISoccerPitchReservationRepository soccerPitchReservationRepository,
            ISoccerPitchRepository soccerPitchRepository,
            IEasySoccerDbContext dbContext,
            ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository,
            ICompanyScheduleRepository companyScheduleRepository)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
            _companyScheduleRepository = companyScheduleRepository;
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

            var selectedSoccerPitch = await _soccerPitchRepository.GetAsync(soccerPitchId);

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
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id,
                Interval = selectedSoccerPitch.Interval
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

        public async Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime selectedDate, int sportType)
        {
            List<AvaliableSchedulesDTO> avaliableSchedules = new List<AvaliableSchedulesDTO>();

            var companySchedule = await _companyScheduleRepository.GetAsync(companyId, (int)selectedDate.DayOfWeek);
            if (companySchedule == null)
                throw new BussinessException("Agenda da empresa não encontrada.");

            if (companySchedule.StartHour > selectedDate.TimeOfDay.Hours && companySchedule.FinalHour < selectedDate.TimeOfDay.Hours)
                throw new BussinessException("Agenda da empresa não permite a hora selecionada.");

            var soccerPitchsBySportType = await _soccerPitchRepository.GetAsync(companyId, sportType);
            AvaliableSchedulesDTO userSelectedSchedule = null;
            foreach (var item in soccerPitchsBySportType)
            {
                var reservationIsAvaliable = await this.CheckReservationIsAvaliable(selectedDate, item.Id, selectedDate.TimeOfDay);
                if (reservationIsAvaliable.IsAvaliable)
                {
                    if (userSelectedSchedule == null)
                    {
                        userSelectedSchedule = new AvaliableSchedulesDTO
                        {
                            IsCurrentSchedule = true,
                            SelectedDate = selectedDate,
                            SelectedHourStart = reservationIsAvaliable.StartHour,
                            SelectedHourEnd = reservationIsAvaliable.EndHour,
                            PossibleSoccerPitchs = new List<SoccerPitch>()
                        };
                    }
                    userSelectedSchedule.PossibleSoccerPitchs.Add(item);
                }
            }
            if (userSelectedSchedule != null)
                avaliableSchedules.Add(userSelectedSchedule);

            var currentTime = selectedDate.TimeOfDay;
            int trys = 0;
            while (avaliableSchedules.Count < 5 && trys < 10)
            {
                currentTime = currentTime.Add(TimeSpan.FromHours(1));
                AvaliableSchedulesDTO alternativeSchedule = null;
                if (companySchedule.FinalHour <= currentTime.Hours)
                {
                    currentTime = TimeSpan.FromHours(companySchedule.StartHour);
                    selectedDate = selectedDate.AddDays(1);
                    companySchedule = await _companyScheduleRepository.GetAsync(companyId, (int)selectedDate.DayOfWeek);
                }
                foreach (var item in soccerPitchsBySportType)
                {
                    var reservationIsAvaliable = await this.CheckReservationIsAvaliable(selectedDate, item.Id, currentTime);
                    if (reservationIsAvaliable.IsAvaliable)
                    {
                        if (alternativeSchedule == null)
                        {
                            alternativeSchedule = new AvaliableSchedulesDTO
                            {
                                IsCurrentSchedule = false,
                                SelectedDate = selectedDate,
                                SelectedHourStart = reservationIsAvaliable.StartHour,
                                SelectedHourEnd = reservationIsAvaliable.EndHour,
                                PossibleSoccerPitchs = new List<SoccerPitch>()
                            };
                        }
                        alternativeSchedule.PossibleSoccerPitchs.Add(item);
                    }
                }
                if (alternativeSchedule != null)
                    avaliableSchedules.Add(alternativeSchedule);
                trys++;
            }
            return avaliableSchedules;
        }



        private async Task<CheckReservationIsAvaliableResponse> CheckReservationIsAvaliable(DateTime selectedDate, long soccerPitchId, TimeSpan selectedHourStart)
        {
            var soccerPitch = await _soccerPitchRepository.GetAsync(soccerPitchId);
            if (soccerPitch == null)
                throw new BussinessException("Quadra não encontrada.");

            var companySchedule = await _companyScheduleRepository.GetAsync(soccerPitch.CompanyId, (int)selectedDate.DayOfWeek);
            if (companySchedule == null)
                throw new BussinessException("Agenda da empresa não encontrada.");

            if (companySchedule.StartHour > selectedHourStart.Hours && companySchedule.FinalHour < selectedHourStart.Hours)
                throw new BussinessException("Agenda da empresa não permite a hora selecionada.");

            var interval = soccerPitch.Interval.HasValue ? soccerPitch.Interval : 60;
            TimeSpan selectedHourEnd = selectedHourStart;
            if (selectedHourStart.Hours == 23)
                selectedHourEnd = new TimeSpan(23, 59, 59);
            else
                selectedHourEnd = selectedHourStart.Add(TimeSpan.FromMinutes(Convert.ToDouble(interval)));
            var reservation = await _soccerPitchReservationRepository.GetAsync(selectedDate, selectedHourStart, selectedHourEnd, soccerPitchId);
            if (reservation == null)
                return new CheckReservationIsAvaliableResponse { IsAvaliable = true, SoccerPitch = soccerPitch, StartHour = selectedHourStart, EndHour = selectedHourEnd };
            else
                return new CheckReservationIsAvaliableResponse { IsAvaliable = false, SoccerPitch = soccerPitch, StartHour = selectedHourStart, EndHour = selectedHourEnd };

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

        public Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day, long companyId)
        {
            if (day.HasValue && day.Value > 0)
            {
                return _soccerPitchReservationRepository.GetAsync(month, day.Value, companyId);
            }
            else
            {
                return _soccerPitchReservationRepository.GetAsync(month, companyId);
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

        public Task<List<SoccerPitchReservation>> GetUserSchedulesAsync(Guid userId)
        {
            return _soccerPitchReservationRepository.GetByUserAsync(userId);
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
