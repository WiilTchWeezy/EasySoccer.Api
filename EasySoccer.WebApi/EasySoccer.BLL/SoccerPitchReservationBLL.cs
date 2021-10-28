using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Helper;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IUserTokenRepository _userTokenRepository;
        private ICompanyUserRepository _companyUserRepository;
        private ICompanyUserNotificationBLL _companyUserNotificationBLL;
        private ICompanyRepository _companyRepository;
        private IPersonCompanyRepository _personCompanyRepository;
        private ISoccerPitchPlanRepository _soccerPitchPlanRepository;
        private IPlanGenerationConfigRepository _planGenerationConfigRepository;

        public SoccerPitchReservationBLL
            (ISoccerPitchReservationRepository soccerPitchReservationRepository,
            ISoccerPitchRepository soccerPitchRepository,
            IEasySoccerDbContext dbContext,
            ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository,
            ICompanyScheduleRepository companyScheduleRepository,
            IPersonRepository personRepository,
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            ICompanyUserRepository companyUserRepository,
            ICompanyUserNotificationBLL companyUserNotificationBLL,
            ICompanyRepository companyRepository,
            IPersonCompanyRepository personCompanyRepository,
            ISoccerPitchPlanRepository soccerPitchPlanRepository,
            IPlanGenerationConfigRepository planGenerationConfigRepository)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
            _companyScheduleRepository = companyScheduleRepository;
            _personRepository = personRepository;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserNotificationBLL = companyUserNotificationBLL;
            _companyRepository = companyRepository;
            _personCompanyRepository = personCompanyRepository;
            _soccerPitchPlanRepository = soccerPitchPlanRepository;
            _planGenerationConfigRepository = planGenerationConfigRepository;
        }


        public async Task<SoccerPitchReservationResponse> CreateAsync(long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long? companyUserId, long soccerPitchPlanId, Guid? personCompanyId, ApplicationEnum application)
        {
            ReservationsGeneratedDTO generatedReservations = null;
            var soccerPicthPlanRelation = await _soccerPitchSoccerPitchPlanRepository.GetAsync(soccerPitchId, soccerPitchPlanId);
            if (soccerPicthPlanRelation == null)
                throw new BussinessException("Não foi encontrado o plano ou a quadra.");
            SoccerPitchReservationResponse response = new SoccerPitchReservationResponse();
            //TODO - adicionar Campo na empresa e informar o fuso horário 
            var currentDateTime = DateTime.UtcNow.AddHours(-3);

            if (selectedDate.Date < currentDateTime.Date)
                throw new BussinessException("Não é possível agendar datas menores que a atual.");

            var selectedDateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourStart.Hours, hourStart.Minutes, hourStart.Seconds);
            var selectedDateEnd = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hourFinish.Hours, hourFinish.Minutes, hourFinish.Seconds);

            var selectedSoccerPitch = await _soccerPitchRepository.GetAsync(soccerPitchId);
            if (selectedSoccerPitch == null)
                throw new NotFoundException(soccerPicthPlanRelation, soccerPitchPlanId);

            var reservationAvaliable = await CheckReservationIsAvaliable(selectedDateStart, soccerPitchId, selectedDateEnd);
            if (reservationAvaliable.IsAvaliable == false)
                throw new BussinessException("Horário selecionado não esta disponivel");

            var soccerPitchPlan = await _soccerPitchPlanRepository.GetAsync((int)soccerPitchPlanId);
            if (soccerPitchPlan == null)
                throw new BussinessException("Não foi encontrado o plano.");

            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDateStart = selectedDateStart,
                SelectedDateEnd = selectedDateEnd,
                SoccerPitchId = soccerPitchId,
                Status = StatusEnum.Waiting,
                StatusChangedUserId = companyUserId.HasValue ? companyUserId.Value : (long?)null,
                SoccerPitchSoccerPitchPlanId = soccerPicthPlanRelation.Id,
                Application = application,
                Interval = selectedSoccerPitch.Interval
            };

            var company = await _companyRepository.GetAsync(selectedSoccerPitch.CompanyId);
            if (company == null)
                throw new BussinessException("Empresa não encontrada.");
            if (company.InsertReservationConfirmed || application == ApplicationEnum.MobileAdm || application == ApplicationEnum.WebApp)
                soccerPitchReservation.Status = StatusEnum.Confirmed;

            if (personId.HasValue)
            {
                if (application == ApplicationEnum.MobileUser || application == ApplicationEnum.WebAppUser)
                {
                    var person = await _personRepository.GetByPersonId(personId.Value);
                    if (person == null)
                        throw new BussinessException("Erro ao realizar o agendamento. Erro no cadastro do usuário.");

                    var personCompany = await _personCompanyRepository.GetAsync(person.Email, person.Phone, company.Id);
                    if (personCompany != null)
                    {
                        soccerPitchReservation.PersonCompanyId = personCompany.Id;
                        if (person.Id != personCompany.PersonId)
                            personCompany.PersonId = person.Id;
                    }
                    else
                    {
                        var createdPersonCompany = new PersonCompany
                        {
                            Phone = person.Phone,
                            CompanyId = company.Id,
                            CreatedDate = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                            Email = person.Email,
                            Name = person.Name,
                            PersonId = person.Id
                        };
                        await _personCompanyRepository.Create(createdPersonCompany);
                        soccerPitchReservation.PersonCompanyId = createdPersonCompany.Id;
                    }

                }
            }
            else
            {
                if (application == ApplicationEnum.MobileUser)
                    throw new BussinessException("É necessário estar autenticado para realizar um agendamento.");
            }
            if (personCompanyId.HasValue)
            {
                var personCompany = await _personCompanyRepository.GetAsync(personCompanyId.Value);
                if (personCompany == null)
                    throw new BussinessException("Cliente não encontrado.");
                soccerPitchReservation.PersonCompanyId = personCompany.Id;
            }


            var validationResponse = ValidationHelper.Instance.Validate(soccerPitchReservation);
            if (validationResponse.IsValid == false)
                throw new BussinessException(validationResponse.ErrorFormatted);
            if (soccerPitchPlan.IdPlanGenerationConfig.HasValue)
            {
                var planGeneration = await _planGenerationConfigRepository.GetAsync(soccerPitchPlan.IdPlanGenerationConfig.Value);
                if (planGeneration != null)
                {
                    generatedReservations = await this.GenerateReservations(soccerPitchPlan, soccerPitchReservation, selectedSoccerPitch, planGeneration);
                    response.ChildReservations = generatedReservations;
                }
            }
            if (generatedReservations != null && generatedReservations.ReservationsEntity != null && generatedReservations.ReservationsEntity.Count > 0)
            {
                foreach (var item in generatedReservations.ReservationsEntity)
                {
                    item.Status = soccerPitchReservation.Status;
                    await _soccerPitchReservationRepository.Create(item);
                }
                response.ChildReservations.ReservationsEntity = null;
            }
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            if (application == ApplicationEnum.MobileUser || application == ApplicationEnum.WebAppUser)
            {
                var users = await _companyUserRepository.GetByCompanyIdAsync(selectedSoccerPitch.CompanyId);
                foreach (var item in users)
                {
                    var data = JsonConvert.SerializeObject(new { reservationId = soccerPitchReservation.Id });
                    var message = string.Format("Um novo horário foi agendado no seu complexo esportivo, na quadra {0}. Acesse seu calendário para mais informações.", selectedSoccerPitch.Name);
                    await _companyUserNotificationBLL.CreateCompanyUserNotificationAsync(item.Id, "Novo horário agendado.", message, Entities.Enum.NotificationTypeEnum.NewReservation, data);
                }
            }
            response.Id = soccerPitchReservation.Id;
            return response;
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(date, companyPitchs, page, pageSize);
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(long companyId, int page, int pageSize, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status)
        {
            var companyPitchs = await _soccerPitchRepository.GetByCompanyIdAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(companyPitchs, page, pageSize, initialDate, finalDate, soccerPitchId, soccerPitchPlanId, userName, status);
        }

        public async Task<List<AvaliableSchedulesDTO>> GetAvaliableSchedules(long companyId, DateTime selectedDate, int sportType)
        {
            List<AvaliableSchedulesDTO> avaliableSchedules = new List<AvaliableSchedulesDTO>();

            //TODO - adicionar Campo na empresa e informar o fuso horário 
            var currentDateTime = DateTime.UtcNow.AddHours(-3);
            if (selectedDate.Date < currentDateTime.Date)
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

            if (soccerPitch.Active == false)
                throw new BussinessException("Quadra não esta ativa.");

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

            if (soccerPitch.Active == false)
                throw new BussinessException("Quadra não esta ativa.");

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

        public Task<List<SoccerPitchReservation>> GetReservationsByMonthOrDay(int month, int? day, long companyId, int year, List<long> socerPitches, List<int> status)
        {
            if (day.HasValue && day.Value > 0)
            {
                return _soccerPitchReservationRepository.GetAsync(month, day.Value, companyId, socerPitches, status);
            }
            else
            {
                return _soccerPitchReservationRepository.GetAsync(month, companyId, year, socerPitches, status);
            }
        }

        public Task<List<SoccerPitchReservation>> GetResumeAsync()
        {
            return _soccerPitchReservationRepository.GetResumeAsync();
        }

        public Task<int> GetTotalAsync(long companyId, DateTime? initialDate, DateTime? finalDate, int? soccerPitchId, int? soccerPitchPlanId, string userName, StatusEnum[] status)
        {
            return _soccerPitchReservationRepository.GetTotalAsync(companyId, initialDate, finalDate, soccerPitchId, soccerPitchPlanId, userName, status);
        }

        public async Task<List<SoccerPitchReservation>> GetUserSchedulesAsync(Guid userId, int page, int pageSize)
        {
            var person = await _personRepository.GetByUserIdAsync(userId);
            if (person != null)
            {
                var personCompany = await _personCompanyRepository.GetByPersonIdAsync(person.Id);
                if (personCompany != null && personCompany.PersonId.HasValue)
                {
                    return await _soccerPitchReservationRepository.GetByPersonCompanyAsync(personCompany.Id, page, pageSize);
                }
            }
            return new List<SoccerPitchReservation>();
        }

        public async Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid? personId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long soccerPitchSoccerPitchPlanId, Guid? personCompanyId)
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
            if (personCompanyId.HasValue)
            {
                var personCompany = await _personCompanyRepository.GetAsync(personCompanyId.Value);
                if (personCompany != null)
                {
                    soccerPitchReservation.PersonCompanyId = personCompany.Id;
                    var person = await _personRepository.GetAsync(personCompany.Email, personCompany.Phone);
                    if (person != null)
                    {
                        personCompany.PersonId = person.Id;
                        await _personCompanyRepository.Edit(personCompany);
                    }
                }
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
                var companyCurrentDate = DateTime.UtcNow.AddHours(-3);
                var avaliables = CheckAvaliableSoccerPitches(companySchedule, soccerPitchs, reservations, selectDate);
                for (int i = (int)companySchedule?.StartHour; i <= (int)companySchedule?.FinalHour; i++)
                {
                    var selectedDateStartHour = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, i, 59, 00);
                    if (companyCurrentDate <= selectedDateStartHour)
                    {
                        response.Add(new GetSchedulesResponse
                        {
                            Hour = i,
                            HourSpan = TimeSpan.FromHours(i),
                            Events = reservations.Where(x => x.SelectedDateStart.TimeOfDay.Hours == i).Select(x => new GetSchedulesResponseEvents
                            {
                                PersonName = x.PersonCompany != null ? x.PersonCompany.Name : "",
                                ScheduleHour = $"{x.SelectedDateStart.TimeOfDay.Hours:00}:{x.SelectedDateStart.TimeOfDay.Minutes:00}  - {x.SelectedDateEnd.TimeOfDay.Hours:00}:{x.SelectedDateEnd.TimeOfDay.Minutes:00} ",
                                SoccerPitch = x.SoccerPitch.Name,
                                HasReservation = true,
                                SoccerPitchId = x.SoccerPitchId,
                                SoccerPitchReservationId = x.Id
                            }).ToList(),
                            AllSoccerPitchesOcupied = avaliables.All(x => x.IsAvaliable == false),
                            FreeSoccerPitches = avaliables.Where(x => x.IsAvaliable)
                                                .Select(y => new SoccerPitchResponse
                                                {
                                                    Name = y.SoccerPitch.Name,
                                                    Id = y.SoccerPitch.Id,
                                                    Interval = y.SoccerPitch.Interval.HasValue && y.SoccerPitch.Interval.Value > 0 ? y.SoccerPitch.Interval.Value : 60,
                                                    AvaliableHours = y.AvaliableHours
                                                }).ToList()
                        });
                    }
                }
            }
            foreach (var item in response)
            {
                if (item.Events.Count > 0)
                {
                    foreach (var soccerPitch in soccerPitchs)
                    {
                        if (item.Events.Where(x => x.SoccerPitchId == soccerPitch.Id).Any() == false)
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

        private List<SoccerPitchesAvailableResponse> CheckAvaliableSoccerPitches(CompanySchedule schedule, List<SoccerPitch> soccerPitches, List<SoccerPitchReservation> reservations, DateTime selectedDate)
        {
            var response = new List<SoccerPitchesAvailableResponse>();
            //TODO - adicionar Campo na empresa e informar o fuso horário 
            var currentDateTime = DateTime.UtcNow.AddHours(-3);
            foreach (var item in soccerPitches)
            {
                var itemResponse = new SoccerPitchesAvailableResponse();
                itemResponse.SoccerPitch = item;
                var intervalsToOffer = new List<int>() { 0, 30 }; // TODO: Create a field on this 
                for (int i = (int)schedule?.StartHour; i <= (int)schedule?.FinalHour; i++)
                {
                    foreach (var intervalToOffer in intervalsToOffer)
                    {
                        var dateStart = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, i, intervalToOffer, 00);
                        var dateEnd = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, i, intervalToOffer, 00).AddMinutes(item.Interval.HasValue && item.Interval.Value > 0 ? item.Interval.Value : 60);
                        var hasReservation = reservations.Where(x =>
                        x.SoccerPitchId == item.Id &&
                        x.Status == StatusEnum.Confirmed &&
                        (
                        (x.SelectedDateStart >= dateStart && x.SelectedDateStart <= dateEnd)
                        ||
                        (x.SelectedDateEnd > dateStart && x.SelectedDateEnd <= dateEnd))
                        ).Any();
                        if (hasReservation == false && dateStart > currentDateTime)
                        {
                            itemResponse.AvaliableHours.Add(new AvaliableHour { Hour = i, HourStart = dateStart.TimeOfDay, HourEnd = dateEnd.TimeOfDay });
                        }
                    }
                }
                response.Add(itemResponse);
            }
            return response;
        }

        public Task<SoccerPitchReservation> GetReservationInfoAsync(Guid reservationId)
        {
            return _soccerPitchReservationRepository.GetAsync(reservationId, true, true);
        }

        public async Task<bool> ChangeStatusAsync(Guid reservationId, StatusEnum status, long userId)
        {
            bool response = false;
            var reservation = await _soccerPitchReservationRepository.GetAsync(reservationId);
            if (reservation == null)
                throw new BussinessException("Agendamento não encontrado.");
            if (reservation.Status == StatusEnum.Concluded)
                throw new BussinessException("Não é possivel alterar o status de um agendamento finalizado.");
            if (status == StatusEnum.Confirmed)
            {
                var avaliableResponse = await CheckReservationIsAvaliable(reservation.SelectedDateStart, reservation.SoccerPitchId, reservation.SelectedDateEnd);
                if (avaliableResponse.IsAvaliable == false)
                    throw new BussinessException("Existe outro agendamento confirmado nesta data e horário.");
            }
            reservation.Status = status;
            reservation.StatusChangedUserId = userId;
            reservation.ModifiedDate = DateTime.UtcNow;
            await _soccerPitchReservationRepository.Edit(reservation);
            await _dbContext.SaveChangesAsync();
            //TODO notificar person se status é confirmado ou cancelado
            return response;
        }

        private async Task<ReservationsGeneratedDTO> GenerateReservations(SoccerPitchPlan soccerPitchPlan, SoccerPitchReservation originReservation, SoccerPitch soccerPitch, PlanGenerationConfig planGenerationConfig)
        {
            ReservationsGeneratedDTO response = new ReservationsGeneratedDTO();
            response.OriginReservation = new ReservationsGenerated();
            response.OriginReservation.SelectedDateStart = originReservation.SelectedDateStart;
            response.OriginReservation.SelectedDateEnd = originReservation.SelectedDateEnd;
            response.OriginReservation.SoccerPitchId = originReservation.SoccerPitchId;
            response.OriginReservation.SoccerPitchName = soccerPitch.Name;
            response.OriginReservation.SoccerPitchPlanId = soccerPitchPlan.Id;
            response.OriginReservation.SoccerPitchPlanName = soccerPitchPlan.Name;
            if (planGenerationConfig.IntervalBetweenReservations > 0)
            {
                response.Reservations = new List<ReservationsGenerated>();
                response.ReservationsEntity = new List<SoccerPitchReservation>();

                bool continueGenerating = true;
                DateTime lastReservationDateStart = originReservation.SelectedDateStart;
                var originReservationInterval = (originReservation.SelectedDateEnd - originReservation.SelectedDateStart).TotalMinutes;
                while (continueGenerating)
                {
                    lastReservationDateStart = lastReservationDateStart.AddDays(planGenerationConfig.IntervalBetweenReservations);
                    var dateStart = lastReservationDateStart;
                    var dateEnd = dateStart.AddMinutes(originReservationInterval);
                    var reservation = new SoccerPitchReservation
                    {
                        SoccerPitchId = soccerPitch.Id,
                        CreatedDate = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        SelectedDateStart = dateStart,
                        SelectedDateEnd = dateEnd,
                        Status = originReservation.Status,
                        PersonCompanyId = originReservation.PersonCompanyId,
                        SoccerPitchSoccerPitchPlanId = originReservation.SoccerPitchSoccerPitchPlanId,
                        Application = originReservation.Application,
                        Interval = originReservation.Interval,
                        OringinReservationId = originReservation.Id
                    };
                    var avaliableResponse = await CheckReservationIsAvaliable(reservation.SelectedDateStart, reservation.SoccerPitchId, reservation.SelectedDateEnd);
                    if (avaliableResponse.IsAvaliable)
                        response.ReservationsEntity.Add(reservation);
                    var reservationDTO = new ReservationsGenerated
                    {
                        SelectedDateEnd = reservation.SelectedDateEnd,
                        SelectedDateStart = reservation.SelectedDateStart,
                        SoccerPitchId = reservation.SoccerPitchId,
                        SoccerPitchName = soccerPitch.Name,
                        SoccerPitchPlanId = soccerPitchPlan.Id,
                        SoccerPitchPlanName = soccerPitchPlan.Name,
                        InsertedSucess = avaliableResponse.IsAvaliable
                    };
                    response.Reservations.Add(reservationDTO);
                    if (planGenerationConfig.LimitType == LimitTypeEnum.DaysAfterFirstReservation)
                        continueGenerating = (reservation.SelectedDateStart - originReservation.SelectedDateStart).TotalDays < planGenerationConfig.LimitQuantity;
                    else if (planGenerationConfig.LimitType == LimitTypeEnum.TotalReservations)
                        continueGenerating = response.Reservations.Count < planGenerationConfig.LimitQuantity;
                    else if (planGenerationConfig.LimitType == LimitTypeEnum.FillCurrentMonth)
                    {
                        var nextDate = reservationDTO.SelectedDateStart.AddDays(planGenerationConfig.IntervalBetweenReservations);
                        continueGenerating = nextDate.Month + nextDate.Year == originReservation.SelectedDateStart.Month + originReservation.SelectedDateStart.Year;
                    }

                }

            }
            return response;
        }

        public async Task<ReservationsGeneratedDTO> GetReservationsGeneratedAsync(Guid id)
        {
            var response = new ReservationsGeneratedDTO();
            var reservation = await _soccerPitchReservationRepository.GetAsync(id, false, true);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada");
            var reservations = new List<SoccerPitchReservation>();
            if (reservation.OringinReservationId.HasValue)
            {
                var originReservation = await _soccerPitchReservationRepository.GetAsync(reservation.OringinReservationId.Value);
                if (originReservation == null)
                    throw new BussinessException("Reserva não encontrada");
                response.OriginReservation = new ReservationsGenerated
                {
                    SelectedDateEnd = originReservation.SelectedDateEnd,
                    SelectedDateStart = originReservation.SelectedDateStart,
                    SoccerPitchId = originReservation.SoccerPitchId,
                    SoccerPitchName = originReservation.SoccerPitch.Name,
                    SoccerPitchPlanId = originReservation.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId,
                    SoccerPitchPlanName = originReservation.SoccerPitchSoccerPitchPlan.SoccerPitchPlan.Name
                };
                reservations = await _soccerPitchReservationRepository.GetByOriginReservationAsync(reservation.OringinReservationId.Value);
            }
            else
            {
                response.OriginReservation = new ReservationsGenerated
                {
                    SelectedDateEnd = reservation.SelectedDateEnd,
                    SelectedDateStart = reservation.SelectedDateStart,
                    SoccerPitchId = reservation.SoccerPitchId,
                    SoccerPitchName = reservation.SoccerPitch.Name,
                    SoccerPitchPlanId = reservation.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId,
                    SoccerPitchPlanName = reservation.SoccerPitchSoccerPitchPlan.SoccerPitchPlan.Name
                };
                reservations = await _soccerPitchReservationRepository.GetByOriginReservationAsync(reservation.Id);
            }
            if (reservations != null && reservations.Count > 0)
            {
                response.Reservations = reservations.Select(x => new ReservationsGenerated
                {
                    SelectedDateEnd = x.SelectedDateEnd,
                    SelectedDateStart = x.SelectedDateStart,
                    SoccerPitchId = x.SoccerPitchId,
                    SoccerPitchName = x.SoccerPitch.Name,
                    SoccerPitchPlanId = x.SoccerPitchSoccerPitchPlan.SoccerPitchPlanId,
                    SoccerPitchPlanName = x.SoccerPitchSoccerPitchPlan.SoccerPitchPlan.Name
                }).ToList();
            }
            return response;
        }

        public async Task<decimal> GetReservationValueAsync(Guid soccerPitchReservationId)
        {
            var reservation = await _soccerPitchReservationRepository.GetAsync(soccerPitchReservationId);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada.");
            var planRelation = await _soccerPitchSoccerPitchPlanRepository.GetByIdAsync(reservation.SoccerPitchSoccerPitchPlanId);
            if(planRelation == null)
                throw new BussinessException("Plano da reserva não encontrado.");
            var soccerPitchPlan = await _soccerPitchPlanRepository.GetAsync(planRelation.SoccerPitchPlanId);
            if (soccerPitchPlan == null)
                throw new BussinessException("Plano da reserva não encontrado.");
            return soccerPitchPlan.Value;
        }
    }
}
