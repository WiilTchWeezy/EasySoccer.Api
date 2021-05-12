using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Helper;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.BLL.Infra.Helpers;
using EasySoccer.BLL.Infra.Services.Azure;
using EasySoccer.BLL.Infra.Services.Azure.Enums;
using EasySoccer.BLL.Infra.Services.Cryptography;
using EasySoccer.BLL.Infra.Services.SendGrid;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class CompanyBLL : ICompanyBLL
    {
        private ICompanyRepository _companyRepository;
        private IEasySoccerDbContext _dbContext;
        private ICompanyScheduleRepository _companyScheduleRepository;
        private IBlobStorageService _blobStorageService;
        private IFormInputRepository _formInputRepository;
        private IEmailService _emailService;
        private ICompanyUserRepository _companyUserRepository;
        private ICompanyUserNotificationRepository _companyUserNotificationRepository;
        private IConfiguration _configuration;
        private ICompanyFinancialRecordRepository _companyFinancialRecordRepository;
        private ICityRepository _cityRepository;
        private IStateRepository _stateRepository;
        private ICryptographyService _cryptographyService;
        private int daysFree = 0;
        private string formContactReceiverName = string.Empty;
        private string formContactReceiverEmail = string.Empty;
        public CompanyBLL
            (ICompanyRepository companyRepository,
            IEasySoccerDbContext dbContext,
            ICompanyScheduleRepository companyScheduleRepository,
            IBlobStorageService blobStorageService,
            IFormInputRepository formInputRepository,
            IEmailService emailService,
            ICompanyUserRepository companyUserRepository,
            IConfiguration configuration,
            ICompanyFinancialRecordRepository companyFinancialRecordRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICryptographyService cryptographyService,
            ICompanyUserNotificationRepository companyUserNotificationRepository)
        {
            _companyRepository = companyRepository;
            _dbContext = dbContext;
            _companyScheduleRepository = companyScheduleRepository;
            _blobStorageService = blobStorageService;
            _formInputRepository = formInputRepository;
            _emailService = emailService;
            _companyUserRepository = companyUserRepository;
            _configuration = configuration;
            _companyFinancialRecordRepository = companyFinancialRecordRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _cryptographyService = cryptographyService;
            _companyUserNotificationRepository = companyUserNotificationRepository;
            var financialConfig = configuration.GetSection("FinancialConfiguration");
            if (financialConfig != null)
            {
                daysFree = financialConfig.GetValue<int>("DaysFree");
            }
            var sendGridConfig = configuration.GetSection("SendGridConfiguration");
            if (sendGridConfig != null)
            {
                formContactReceiverName = sendGridConfig.GetValue<string>("ContactReceiverName");
                formContactReceiverEmail = sendGridConfig.GetValue<string>("ContactReceiverEmail");
            }
        }

        public async Task<Company> CreateAsync(string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude)
        {
            var company = new Company
            {
                CNPJ = cnpj,
                CreatedDate = DateTime.UtcNow,
                Description = description,
                Latitude = latitude,
                Longitude = longitude,
                Name = name,
                WorkOnHoliDays = workOnHolidays
            };

            await _companyRepository.Create(company);
            await _dbContext.SaveChangesAsync();
            return company;
        }

        public async Task<List<Company>> GetAsync(double? longitude, double? latitude, int page, int pageSize, string name, string orderField, string orderDirection)
        {
            var companies = await _companyRepository.GetAsync(page, pageSize, name, orderField, orderDirection);

            if (orderField == "Location")
            {
                if (longitude.HasValue && latitude.HasValue)
                {
                    foreach (var item in companies)
                    {
                        item.Distance = LocationHelper.Haversine(longitude.Value, latitude.Value, (double)item.Longitude, (double)item.Latitude);
                    }
                    return companies.OrderBy(c => c.Distance).ToList();
                }
            }
            return companies;
        }

        public Task<Company> GetAsync(long companyId)
        {
            return _companyRepository.GetAsync(companyId);
        }

        public async Task SaveFormInputCompanyAsync(FormInputCompanyEntryRequest request)
        {
            var formInput = new FormInput
            {
                CreatedDate = DateTime.UtcNow,
                FormType = FormTypeEnum.CompanyLandingPageEntry,
                InputData = JsonConvert.SerializeObject(request),
                Status = FormStatusEnum.Inserted,
                Message = "Registro inserido aguardando processamento."
            };
            await _formInputRepository.Create(formInput);
            await _dbContext.SaveChangesAsync();

            var validationResponse = await ValidationHelper.Instance.Validate(request, _companyUserRepository, _companyRepository, daysFree);

            if (validationResponse.IsValid == false)
            {
                formInput.Message = "Erro na validação dos dados. - " + validationResponse.ErrorHtmlFormatted;
                formInput.Status = Entities.Enum.FormStatusEnum.Error;
                await _dbContext.SaveChangesAsync();
                await _emailService.SendValidationErrorsEmailAsync(request.UserEmail, request.UserName, validationResponse.ErrorHtmlFormatted);
            }
            else
            {
                try
                {
                    var company = new Company
                    {
                        Active = false,
                        CNPJ = request.CompanyDocument.Replace(".", "").Replace("-", "").Replace("/", ""),
                        Name = request.CompanyName,
                        Description = request.CompanyName,
                        CreatedDate = DateTime.UtcNow
                    };
                    await _companyRepository.Create(company);
                    await _dbContext.SaveChangesAsync();
                    foreach (var item in this.GetDefaultCompanySchedules(company.Id))
                    {
                        await _companyScheduleRepository.Create(item);
                    }
                    var companyUser = new CompanyUser
                    {
                        CreatedDate = DateTime.UtcNow,
                        Name = request.UserName,
                        CompanyId = company.Id,
                        Email = request.UserEmail,
                        Password = _cryptographyService.Encrypt(request.Password)
                    };
                    await _companyUserRepository.Create(companyUser);

                    var companyUserNotification = new CompanyUserNotification
                    {
                        Active = true,
                        CreatedDate = DateTime.UtcNow,
                        Data = string.Empty,
                        Id = Guid.NewGuid(),
                        IdCompanyUser = companyUser.Id,
                        Message = "Seja bem-vindo ao EasySoccer! Em caso de problemas ou sugestões entre em contato conosco atraves do menu ajuda.",
                        NotificationType = NotificationTypeEnum.Standard,
                        Title = "Seja bem-vindo ao EasySoccer!",
                        Read = false
                    };
                    await _companyUserNotificationRepository.Create(companyUserNotification);
                    if (daysFree > 0)
                    {
                        var companyFinancialRecord = new CompanyFinancialRecord
                        {
                            CompanyId = company.Id,
                            CreatedDate = DateTime.UtcNow,
                            ExpiresDate = DateTime.UtcNow.AddDays(daysFree),
                            Paid = true,
                            Value = 0,
                            Transaction = null
                        };
                        await _companyFinancialRecordRepository.Create(companyFinancialRecord);
                    }
                    else
                    {
                        var companyFinancialRecord = new CompanyFinancialRecord
                        {
                            CompanyId = company.Id,
                            CreatedDate = DateTime.UtcNow,
                            ExpiresDate = DateTime.UtcNow.AddMonths(FinancialHelper.Instance.GetMonthsFromPlan(request.SelectedPlan.HasValue ? request.SelectedPlan.Value : FinancialPlanEnum.Free)),
                            Paid = true, //TODO - Quando inserir gateway de pagamento inserir false e tratar depois no callBack
                            Value = FinancialHelper.Instance.GetValueFromPlan(request.SelectedPlan.HasValue ? request.SelectedPlan.Value : FinancialPlanEnum.Free),
                            Transaction = null,
                            FinancialPlan = request.SelectedPlan.HasValue ? request.SelectedPlan.Value : FinancialPlanEnum.Free
                        };
                        await _companyFinancialRecordRepository.Create(companyFinancialRecord);
                        //TODO - Payment Gateway insert
                    }
                    await _dbContext.SaveChangesAsync();
                    await _emailService.SendSuccessEmailAsync(request.UserEmail, request.UserName, daysFree, companyUser.Password);
                }
                catch (Exception e)
                {
                    formInput.Message = "Erro ao salvar os dados. - " + e.Message;
                    formInput.Status = Entities.Enum.FormStatusEnum.Error;
                    await _dbContext.SaveChangesAsync();
                    throw e;
                }
            }

        }



        private List<CompanySchedule> GetDefaultCompanySchedules(long companyId)
        {
            var companySchedules = new List<CompanySchedule>();
            for (int i = 0; i < 7; i++)
            {
                companySchedules.Add(new CompanySchedule
                {
                    CompanyId = companyId,
                    CreatedDate = DateTime.UtcNow,
                    Day = i,
                    StartHour = 13,
                    FinalHour = 23,
                    WorkOnThisDay = true
                });
            }
            return companySchedules;
        }

        public async Task SaveImageAsync(long companyId, string imageBase64)
        {
            var currentCompany = await _companyRepository.GetAsync(companyId);
            if (currentCompany == null)
                throw new BussinessException("Empresa não encontrada!");
            try
            {
                if (string.IsNullOrEmpty(currentCompany.Logo) == false)
                    _blobStorageService.Delete(currentCompany.Logo, BlobContainerEnum.CompanyContainer);
            }
            catch (Exception e)
            {

            }

            var bytes = Convert.FromBase64String(imageBase64);
            var fileName = await _blobStorageService.Save(bytes, BlobContainerEnum.CompanyContainer);
            currentCompany.Logo = fileName;
            await _companyRepository.Edit(currentCompany);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude, string completeAddress, List<CompanySchedulesRequest> companySchedules, int? idCity, bool insertReservationConfirmed)
        {
            var currentCompany = await _companyRepository.GetAsync(id);
            if (currentCompany == null)
                throw new NotFoundException(currentCompany, id);
            currentCompany.Name = name;
            currentCompany.Description = description;
            currentCompany.CompleteAddress = completeAddress;
            currentCompany.WorkOnHoliDays = workOnHolidays;
            if (longitude.HasValue)
                currentCompany.Longitude = longitude;
            if (latitude.HasValue)
                currentCompany.Latitude = latitude;
            if (idCity.HasValue)
            {
                var city = await _cityRepository.GetAsync(idCity.Value);
                if (city != null)
                {
                    currentCompany.IdCity = city.Id;
                }
            }
            currentCompany.InsertReservationConfirmed = insertReservationConfirmed;
            await _companyRepository.Edit(currentCompany);
            await _dbContext.SaveChangesAsync();

            if (companySchedules != null)
            {
                foreach (var item in companySchedules)
                {
                    var currentSchedule = await _companyScheduleRepository.GetAsync(item.CompanyId, item.Day);
                    if (currentSchedule == null)
                        throw new NotFoundException(item.CompanyId, item.Day);
                    currentSchedule.StartHour = item.StartHour;
                    currentSchedule.FinalHour = item.FinalHour;
                    currentSchedule.WorkOnThisDay = item.WorkOnThisDay;
                    await _companyScheduleRepository.Edit(currentSchedule);
                }
            }
            await _dbContext.SaveChangesAsync();

            return currentCompany;
        }

        public async Task SaveFormInputContactAsync(FormInputContactRequest request)
        {
            var formInput = new FormInput
            {
                CreatedDate = DateTime.UtcNow,
                FormType = Entities.Enum.FormTypeEnum.ContactLandingPageEntry,
                InputData = JsonConvert.SerializeObject(request),
                Status = Entities.Enum.FormStatusEnum.Inserted,
                Message = "Registro inserido aguardando processamento."
            };
            try
            {
                await _formInputRepository.Create(formInput);
                await _dbContext.SaveChangesAsync();
                await _emailService.SendTextEmailAsync(formContactReceiverEmail, formContactReceiverName, string.Format(" Novo contato recebido - {0} - Nome:{1} - Email: {2}", request.Subject, request.Name, request.Email), String.Format("Nome: {0} - Email: {1} Mensagem: {2}", request.Name, request.Email, request.Message));
                formInput.Status = FormStatusEnum.Success;
                formInput.Message = "Registro Processado";
                await _formInputRepository.Edit(formInput);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                formInput.Status = FormStatusEnum.Error;
                formInput.Message = e.Message;
                await _formInputRepository.Edit(formInput);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ActiveAsync(long companyId, bool active)
        {
            var currentCompany = await _companyRepository.GetAsync(companyId);
            if (currentCompany != null)
            {
                if (active != currentCompany.Active)
                {
                    currentCompany.Active = active;
                    await _companyRepository.Edit(currentCompany);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public Task<List<City>> GetCitiesByState(int IdState)
        {
            return _cityRepository.GetCitiesByState(IdState);
        }

        public Task<List<State>> GetStates()
        {
            return _stateRepository.GetAsync();
        }

        public Task<CompanyFinancialRecord> GetCurrentFinancialInfoAsync(long companyId)
        {
            return _companyFinancialRecordRepository.GetByCompanyAsync(companyId);
        }

        public async Task SaveFormIndicateCompanyAsync(SaveFormIndicateCompanyRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Comment) || string.IsNullOrEmpty(request.CompanyEmail) || string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.CompanyNumber))
                throw new BussinessException("É necessário informar algum campo");
            if (string.IsNullOrEmpty(request.CompanyName))
                throw new BussinessException("É necessário informar o nome da empresa");
            var formInput = new FormInput
            {
                CreatedDate = DateTime.UtcNow,
                FormType = Entities.Enum.FormTypeEnum.CompanyIndicateMobileEntry,
                InputData = JsonConvert.SerializeObject(request),
                Status = Entities.Enum.FormStatusEnum.Inserted,
                Message = "Registro inserido aguardando processamento."
            };
            try
            {
                formInput.Status = FormStatusEnum.Success;
                formInput.Message = "Registro Processado";
                await _emailService.SendTextEmailAsync(formContactReceiverEmail, formContactReceiverName, string.Format(" Nova Indicação de Empresa"), String.Format("Nome: {0} - Email: {1} Telefone: {2} Comentário: {3}", request.CompanyName, request.CompanyEmail, request.CompanyNumber, request.Comment));
                await _formInputRepository.Create(formInput);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                formInput.Status = FormStatusEnum.Error;
                formInput.Message = e.Message;
                await _formInputRepository.Edit(formInput);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
