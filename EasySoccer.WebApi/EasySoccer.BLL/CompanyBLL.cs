using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Helper;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.BLL.Infra.Services.Azure;
using EasySoccer.BLL.Infra.Services.Azure.Enums;
using EasySoccer.BLL.Infra.Services.SendGrid;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
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
        public CompanyBLL(ICompanyRepository companyRepository, IEasySoccerDbContext dbContext, ICompanyScheduleRepository companyScheduleRepository, IBlobStorageService blobStorageService, IFormInputRepository formInputRepository, IEmailService emailService)
        {
            _companyRepository = companyRepository;
            _dbContext = dbContext;
            _companyScheduleRepository = companyScheduleRepository;
            _blobStorageService = blobStorageService;
            _formInputRepository = formInputRepository;
            _emailService = emailService;
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

        public async Task<List<Company>> GetAsync(double? longitude, double? latitude, string description, int page, int pageSize)
        {
            var companies = await _companyRepository.GetAsync(description, page, pageSize);

            if (longitude.HasValue && latitude.HasValue)
            {
                foreach (var item in companies)
                {
                    item.Distance = LocationHelper.Haversine(longitude.Value, latitude.Value, (double)item.Longitude, (double)item.Latitude);
                }
                return companies.OrderBy(c => c.Distance).ToList();
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
                FormType = Entities.Enum.FormTypeEnum.CompanyLandingPageEntry,
                InputData = JsonConvert.SerializeObject(request),
                Status = Entities.Enum.FormStatusEnum.Inserted,
                Message = "Registro inserido aguardando processamento."
            };
            await _formInputRepository.Create(formInput);
            await _dbContext.SaveChangesAsync();
            var errors = new List<string>();

            errors.Add(ValidationHelper.Instance.ValidateEmail(request.UserEmail));
            errors.Add(ValidationHelper.Instance.ValidateCompanyDocument(request.CompanyDocument));
            errors.Add(ValidationHelper.Instance.ValidateCardNumberAndSecurityCode(request.CardNumber, request.SecurityCode));
            errors.Add(ValidationHelper.Instance.ValidateUserDocument(request.FinancialDocument));

            if (errors != null && errors.Count > 0 && errors.Any(x => string.IsNullOrEmpty(x) == false))
            {
                string errorFormatted = "";
                foreach (var item in errors)
                {
                    errorFormatted += item;
                }
                formInput.Message = "Erro na validação dos dados. - " + errorFormatted;
                formInput.Status = Entities.Enum.FormStatusEnum.Error;
                await _dbContext.SaveChangesAsync();
                if (string.IsNullOrEmpty(ValidationHelper.Instance.ValidateEmail(request.UserEmail)))
                {
                    await _emailService.SendValidationErrorsEmailAsync(request.UserEmail, request.UserName, errors.ToArray());
                }
            }

        }

        public async Task SaveImageAsync(long companyId, string imageBase64)
        {
            var currentCompany = await _companyRepository.GetAsync(companyId);
            if (currentCompany == null)
                throw new BussinessException("Empresa não encontrada!");
            if (string.IsNullOrEmpty(currentCompany.Logo) == false)
                _blobStorageService.Delete(currentCompany.Logo, BlobContainerEnum.CompanyContainer);

            var bytes = Convert.FromBase64String(imageBase64);
            var fileName = await _blobStorageService.Save(bytes, BlobContainerEnum.CompanyContainer);
            currentCompany.Logo = fileName;
            await _companyRepository.Edit(currentCompany);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Company> UpdateAsync(long id, string name, string description, string cnpj, bool workOnHolidays, decimal? longitude, decimal? latitude, string completeAddress, List<CompanySchedulesRequest> companySchedules)
        {
            var currentCompany = await _companyRepository.GetAsync(id);
            if (currentCompany == null)
                throw new NotFoundException(currentCompany, id);
            currentCompany.Name = name;
            currentCompany.Description = description;
            currentCompany.CNPJ = cnpj;
            currentCompany.CompleteAddress = completeAddress;
            currentCompany.WorkOnHoliDays = workOnHolidays;
            if (longitude.HasValue)
                currentCompany.Longitude = longitude;
            if (latitude.HasValue)
                currentCompany.Latitude = latitude;
            await _companyRepository.Edit(currentCompany);
            await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            return currentCompany;
        }
    }
}
