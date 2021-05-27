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
    public class PersonCompanyBLL : IPersonCompanyBLL
    {
        private IPersonCompanyRepository _personCompanyRepository;
        private IEasySoccerDbContext _dbContext;
        private ISoccerPitchReservationRepository _soccerPitchReservationRepository;
        public PersonCompanyBLL(IPersonCompanyRepository personCompanyRepository, ISoccerPitchReservationRepository soccerPitchReservationRepository,IEasySoccerDbContext dbContext)
        {
            _personCompanyRepository = personCompanyRepository;
            _dbContext = dbContext;
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
        }

        public async Task<PersonCompany> CreateAsync(string name, string email, string phone, long companyId)
        {
            var currentPerson = await _personCompanyRepository.GetAsync(email, phone, companyId);
            if (currentPerson != null)
            {
                if (currentPerson.Email == email)
                    throw new BussinessException("Já existe um usuário com este e-mail.");
                if (string.IsNullOrEmpty(phone) == false && currentPerson.Phone == phone)
                    throw new BussinessException("Já existe um usuário com este telefone");
            }
            var createdPerson = new PersonCompany
            {
                Phone = phone,
                CompanyId = companyId,
                CreatedDate = DateTime.UtcNow,
                Email = email,
                Name = name,
                Id = Guid.NewGuid()
            };
            await _personCompanyRepository.Create(createdPerson);
            await _dbContext.SaveChangesAsync();
            return createdPerson;
        }

        public Task<List<PersonCompany>> GetAsync(string name, string email, string phone, int page, int pageSize, long companyId)
        {
            return _personCompanyRepository.GetAsync(name, email, phone, page, pageSize, companyId);
        }

        public Task<int> GetAsync(string name, string email, string phone, long companyId)
        {
            return _personCompanyRepository.GetAsync(name, email, phone, companyId);
        }

        public Task<List<PersonCompany>> GetAutoCompleteAsync(string filter, long companyId)
        {
            return _personCompanyRepository.GetAsync(filter, companyId);
        }

        public async Task<PersonCompanyInfoResponse> GetInfoAsync(Guid id)
        {
            var person = await _personCompanyRepository.GetAsync(id);
            if (person == null)
                throw new BussinessException("Cliente não encontrado!");
            var personCompanyInfo = new PersonCompanyInfoResponse 
            {
                Email = person.Email,
                Id = person.Id,
                Name = person.Name,
                Phone = person.Phone
            };
            var reservations = await _soccerPitchReservationRepository.GetByPersonCompanyAsync(person.Id, 1, 5);
            if(reservations != null && reservations.Any())
            {
                personCompanyInfo.Reservations = reservations.Select(x => new ReservationResponse 
                {
                    CompanyId = x.SoccerPitch.CompanyId,
                    CompanyName = x.SoccerPitch.Company.Name,
                    Id = x.Id,
                    SoccerPitchId = x.SoccerPitchId,
                    SoccerPitchName = x.SoccerPitch.Name,
                    SelectedDateEnd = x.SelectedDateEnd,
                    SelectedDateStart = x.SelectedDateStart
                }).ToList();
            }
            else
            {
                personCompanyInfo.Reservations = new List<ReservationResponse>();
            }

            return personCompanyInfo;
        }

        public async Task<PersonCompany> UpdateAsync(Guid personId, string name, string email, string phone, long companyId)
        {
            var currentPerson = await _personCompanyRepository.GetAsync(personId);
            if (currentPerson == null)
                throw new BussinessException("Cliente não encontrado.");
            if (string.IsNullOrEmpty(email) == false && currentPerson.Email != email)
            {
                var currentPersonByEmail = await _personCompanyRepository.GetByEmailAsync(email, companyId);
                if (currentPersonByEmail != null)
                    throw new BussinessException($"Já existe um cliente cadastrado com este e-mail - {email}.");
                currentPerson.Email = email;
            }
            if (string.IsNullOrEmpty(phone) == false && currentPerson.Phone != phone)
            {
                var currentPersonByPhone = await _personCompanyRepository.GetByPhoneAsync(phone, companyId);
                if (currentPersonByPhone != null)
                    throw new BussinessException($"Já existe um cliente cadastrado com este telefone - {phone}.");
                currentPerson.Phone = phone;
            }
            if (string.IsNullOrEmpty(name) == false && currentPerson.Name != name)
                currentPerson.Name = name;
            await _personCompanyRepository.Edit(currentPerson);
            await _dbContext.SaveChangesAsync();
            return currentPerson;
        }
    }
}
