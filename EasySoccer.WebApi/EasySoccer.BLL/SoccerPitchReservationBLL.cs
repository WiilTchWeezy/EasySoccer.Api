using EasySoccer.BLL.Enums;
using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Helper;
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
        private IPersonRepository _personRepository;
        private IUserRepository _userRepository;
        public SoccerPitchReservationBLL
            (ISoccerPitchReservationRepository soccerPitchReservationRepository,
            ISoccerPitchRepository soccerPitchRepository,
            IEasySoccerDbContext dbContext,
            ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository,
            ICompanyScheduleRepository companyScheduleRepository,
            IPersonRepository personRepository,
            IUserRepository userRepository)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
            _companyScheduleRepository = companyScheduleRepository;
            _personRepository = personRepository;
            _userRepository = userRepository;
        }

        public async Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long companyUserId, long soccerPitchPlanId)
        {
            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchPlanId);
            if (soccerPicthPlanRelation == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);
            var selectedDateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourStart.Hours, hourStart.Minutes, hourStart.Seconds);
            var selectedDateEnd = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourFinish.Hours, hourFinish.Minutes, hourFinish.Seconds);

            var reservationAvaliable = await CheckReservationIsAvaliable(selectedDateStart, soccerPitchId, selectedDateEnd);
            if (reservationAvaliable.IsAvaliable == false)
                throw new BussinessException("Horário selecionado não esta disponivel");

            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDateStart = selectedDateStart,
                SelectedDateEnd = selectedDateEnd,
                SoccerPitchId = soccerPitchId,
                Status = (int)StatusEnum.AguardandoAprovacao,
                StatusChangedUserId = companyUserId,
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id
            };

            if (personId.HasValue)
            {
                var person = await _personRepository.GetByPersonId(personId.Value);
                if (person != null)
                    soccerPitchReservation.PersonId = personId;
            }
            var validationResponse = ValidationHelper.Instance.Validate(soccerPitchReservation);
            if (validationResponse.IsValid == false)
                throw new BussinessException(validationResponse.ErrorFormatted);
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchPlanId)
        {

            if (selectedDate.Date < DateTime.Now.Date)
                throw new BussinessException("Não é possível agendar datas menores que a atual.");

            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchPlanId);
            if (soccerPicthPlanRelation == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);

            var selectedSoccerPitch = await _soccerPitchRepository.GetAsync(soccerPitchId);

            if (selectedSoccerPitch == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);


            var selectedDateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourStart.Hours, hourStart.Minutes, hourStart.Seconds);
            var selectedDateEnd = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourFinish.Hours, hourFinish.Minutes, hourFinish.Seconds);

            var reservationAvaliable = await CheckReservationIsAvaliable(selectedDateStart, soccerPitchId, selectedDateEnd);
            if (reservationAvaliable.IsAvaliable == false)
                throw new BussinessException("Horário selecionado não esta disponivel");
            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDateStart = selectedDateStart,
                SelectedDateEnd = selectedDateEnd,
                SoccerPitchId = soccerPitchId,
                Status = (int)StatusEnum.AguardandoAprovacao,
                StatusChangedUserId = null,
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id,
                Interval = selectedSoccerPitch.Interval
            };
            if (userId != default(Guid))
            {
                var person = await _personRepository.GetByUserIdAsync(userId);
                if (person != null)
                {
                    if (string.IsNullOrEmpty(person.Phone))
                        throw new BussinessException("É necessário preencher um telefone para realizar um agendamento.");
                    soccerPitchReservation.PersonId = person.Id;
                }
            }
            var validationResponse = ValidationHelper.Instance.Validate(soccerPitchReservation);
            if (validationResponse.IsValid == false)
                throw new BussinessException(validationResponse.ErrorFormatted);
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(date, companyPitchs, page, pageSize);
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(long companyId, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(companyPitchs, page, pageSize, initialDate, finalDate, soccerPitchId, soccerPitchPlanId, userName);
        }

        public async Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime selectedDate, int sportType)
        {
            List<AvaliableSchedulesDTO> avaliableSchedules = new List<AvaliableSchedulesDTO>();

            if (selectedDate.Date < DateTime.Now.Date)
                throw new BussinessException("Não é possível verificar datas menores que a atual.");

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
                            SelectedHourStart = reservationIsAvaliable.SelectedDateStart.TimeOfDay,
                            SelectedHourEnd = reservationIsAvaliable.SelectedDateEnd.TimeOfDay,
                            PossibleSoccerPitchs = new List<SoccerPitch>()
                        };
                    }
                    userSelectedSchedule.PossibleSoccerPitchs.Add(item);
                }
            }
            if (userSelectedSchedule != null)
                avaliableSchedules.Add(userSelectedSchedule);


            if (avaliableSchedules.Any(x => x.IsCurrentSchedule) == false)
            {
                var tomorrow = selectedDate.AddDays(1);
                AvaliableSchedulesDTO alternativeSchedule = null;
                AvaliableSchedulesDTO alternativeNextWeekSchedule = null;

                foreach (var item in soccerPitchsBySportType)
                {
                    var reservationIsAvaliable = await this.CheckReservationIsAvaliable(tomorrow, item.Id, tomorrow.TimeOfDay);
                    if (reservationIsAvaliable.IsAvaliable)
                    {
                        if (alternativeSchedule == null)
                        {
                            alternativeSchedule = new AvaliableSchedulesDTO
                            {
                                IsCurrentSchedule = false,
                                SelectedDate = tomorrow,
                                SelectedHourStart = reservationIsAvaliable.SelectedDateStart.TimeOfDay,
                                SelectedHourEnd = reservationIsAvaliable.SelectedDateEnd.TimeOfDay,
                                PossibleSoccerPitchs = new List<SoccerPitch>()
                            };
                        }
                        alternativeSchedule.PossibleSoccerPitchs.Add(item);
                    }
                    var nextWeek = selectedDate.AddDays(7);
                    var reservationNextWeekIsAvaliable = await this.CheckReservationIsAvaliable(nextWeek, item.Id, nextWeek.TimeOfDay);
                    if (reservationNextWeekIsAvaliable.IsAvaliable)
                    {
                        if (alternativeNextWeekSchedule == null)
                        {
                            alternativeNextWeekSchedule = new AvaliableSchedulesDTO
                            {
                                IsCurrentSchedule = false,
                                SelectedDate = nextWeek,
                                SelectedHourStart = reservationNextWeekIsAvaliable.SelectedDateStart.TimeOfDay,
                                SelectedHourEnd = reservationNextWeekIsAvaliable.SelectedDateEnd.TimeOfDay,
                                PossibleSoccerPitchs = new List<SoccerPitch>()
                            };
                        }
                        alternativeNextWeekSchedule.PossibleSoccerPitchs.Add(item);
                    }
                }
                if (alternativeSchedule != null)
                    avaliableSchedules.Add(alternativeSchedule);
                if (alternativeNextWeekSchedule != null)
                    avaliableSchedules.Add(alternativeNextWeekSchedule);
            }

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
                                SelectedHourStart = reservationIsAvaliable.SelectedDateStart.TimeOfDay,
                                SelectedHourEnd = reservationIsAvaliable.SelectedDateEnd.TimeOfDay,
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

            var selectedDateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedHourStart.Hours, selectedHourStart.Minutes, selectedHourStart.Seconds);
            var selectedDateEnd = selectedDateStart.AddMinutes((double)interval);
            var reservation = await _soccerPitchReservationRepository.GetAsync(selectedDateStart, selectedDateEnd, soccerPitchId);
            if (reservation == null)
                return new CheckReservationIsAvaliableResponse { IsAvaliable = true, SoccerPitch = soccerPitch, SelectedDateStart = selectedDateStart, SelectedDateEnd = selectedDateEnd };
            else
                return new CheckReservationIsAvaliableResponse { IsAvaliable = false, SoccerPitch = soccerPitch, SelectedDateStart = selectedDateStart, SelectedDateEnd = selectedDateEnd };

        }

        private async Task<CheckReservationIsAvaliableResponse> CheckReservationIsAvaliable(DateTime selectedDateStart, long soccerPitchId, DateTime selectedDateEnd)
        {
            var soccerPitch = await _soccerPitchRepository.GetAsync(soccerPitchId);
            if (soccerPitch == null)
                throw new BussinessException("Quadra não encontrada.");

            var companySchedule = await _companyScheduleRepository.GetAsync(soccerPitch.CompanyId, (int)selectedDateStart.DayOfWeek);
            if (companySchedule == null)
                throw new BussinessException("Agenda da empresa não encontrada.");

            if (companySchedule.StartHour > selectedDateStart.TimeOfDay.Hours && companySchedule.FinalHour < selectedDateStart.TimeOfDay.Hours)
                throw new BussinessException("Agenda da empresa não permite a hora selecionada.");

            var reservation = await _soccerPitchReservationRepository.GetAsync(selectedDateStart, selectedDateEnd, soccerPitchId);
            if (reservation == null)
                return new CheckReservationIsAvaliableResponse { IsAvaliable = true, SoccerPitch = soccerPitch, SelectedDateStart = selectedDateStart, SelectedDateEnd = selectedDateEnd };
            else
                return new CheckReservationIsAvaliableResponse { IsAvaliable = false, SoccerPitch = soccerPitch, SelectedDateStart = selectedDateStart, SelectedDateEnd = selectedDateEnd, SoccerPitchReservation = reservation };

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

        public Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day, long companyId, int year)
        {
            if (day.HasValue && day.Value > 0)
            {
                return _soccerPitchReservationRepository.GetAsync(month, day.Value, companyId);
            }
            else
            {
                return _soccerPitchReservationRepository.GetAsync(month, companyId, year);
            }
        }

        public Task<List<SoccerPitchReservation>> GetResumeAsync()
        {
            return _soccerPitchReservationRepository.GetResumeAsync();
        }

        public Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName)
        {
            return _soccerPitchReservationRepository.GetTotalAsync(companyId, initialDate, finalDate, soccerPitchId, soccerPitchPlanId, userName);
        }

        public Task<List<SoccerPitchReservation>> GetUserSchedulesAsync(Guid userId)
        {
            return _soccerPitchReservationRepository.GetByUserAsync(userId);
        }

        public async Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId)
        {
            var soccerPitchReservation = await _soccerPitchReservationRepository.GetAsync(id);
            if (soccerPitchReservation == null)
                throw new NotFoundException(soccerPitchReservation, id);

            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchSoccerPitchPlanId);
            if (soccerPicthPlanRelation != null)
                soccerPitchReservation.SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id;


            var selectedDateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourStart.Hours, hourStart.Minutes, hourStart.Seconds);
            var selectedDateEnd = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourFinish.Hours, hourFinish.Minutes, hourFinish.Seconds);

            var reservationAvaliable = await CheckReservationIsAvaliable(selectedDateStart, soccerPitchId, selectedDateEnd);
            if (reservationAvaliable.IsAvaliable == false)
            {
                if (reservationAvaliable.SoccerPitchReservation.Id != soccerPitchReservation.Id)
                    throw new BussinessException("Horário selecionado não esta disponivel");
            }

            soccerPitchReservation.Note = note;
            soccerPitchReservation.SelectedDateStart = selectedDateStart;
            soccerPitchReservation.SelectedDateEnd = selectedDateEnd;
            soccerPitchReservation.SoccerPitchId = soccerPitchId;
            if (personId.HasValue)
            {
                var person = await _personRepository.GetByPersonId(personId.Value);
                if (person != null)
                    soccerPitchReservation.PersonId = personId;
            }
            var validationResponse = ValidationHelper.Instance.Validate(soccerPitchReservation);
            if (validationResponse.IsValid == false)
                throw new BussinessException(validationResponse.ErrorFormatted);
            await _soccerPitchReservationRepository.Edit(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<List<GetSchedulesResponse>> GetSchedulesResponses(long companyId, DateTime selectDate)
        {
            var response = new List<GetSchedulesResponse>();
            var reservations = await _soccerPitchReservationRepository.GetAsync(companyId, selectDate);
            var soccerPitchs = await _soccerPitchRepository.GetByCompanyAsync((int)companyId);
            var companySchedule = await _companyScheduleRepository.GetAsync(companyId, (int)selectDate.DayOfWeek);
            if (companySchedule != null)
            {
                for (int i = (int)companySchedule?.StartHour; i <= (int)companySchedule?.FinalHour; i++)
                {
                    response.Add(new GetSchedulesResponse 
                    { 
                        Hour = $"{i}:00",
                        HourSpan = TimeSpan.FromHours(i),
                        Events = reservations.Where(x => x.SelectedDateStart.TimeOfDay.Hours == i).Select(x => new GetSchedulesResponseEvents 
                        {
                            PersonName = x.Person != null ? x.Person.Name : "",
                            ScheduleHour = $"{x.SelectedDateStart.TimeOfDay.Hours:00}:{x.SelectedDateStart.TimeOfDay.Minutes:00}  - {x.SelectedDateEnd.TimeOfDay.Hours:00}:{x.SelectedDateEnd.TimeOfDay.Minutes:00} ",
                            SoccerPitch = x.SoccerPitch.Name,
                            HasReservation = true,
                            SoccerPitchId = x.SoccerPitchId
                        }).ToList()
                    });
                }
            }
            foreach (var item in response)
            {
                if (item.Events.Count > 0)
                {
                    foreach (var soccerPitch in soccerPitchs)
                    {
                        if(item.Events.Where(x => x.SoccerPitchId == soccerPitch.Id).Any() == false)
                        {
                            var hourEnd = item.HourSpan.Add(TimeSpan.FromMinutes(soccerPitch.Interval.HasValue ? soccerPitch.Interval.Value : 60));
                            item.Events.Add(new GetSchedulesResponseEvents 
                            {
                                ScheduleHour = $"{item.HourSpan.Hours:00}:{item.HourSpan.Minutes:00}  - {hourEnd.Hours:00}:{hourEnd.Minutes:00} ",
                                SoccerPitch = soccerPitch.Name,
                                HasReservation = false,
                                SoccerPitchId = soccerPitch.Id
                            });
                        }
                    }
                }
            }
            return response;
        }
    }
}
