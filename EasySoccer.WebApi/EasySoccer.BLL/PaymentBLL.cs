using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class PaymentBLL : IPaymentBLL
    {
        private IPaymentRepository _paymentRepository;
        private IPersonCompanyRepository _personCompanyRepository;
        private IEasySoccerDbContext _dbContext;
        private ICompanyUserRepository _companyUserRepository;
        private IFormOfPaymentRepository _formOfPaymentRepository;
        public PaymentBLL(IPaymentRepository paymentRepository, IPersonCompanyRepository personCompanyRepository, IEasySoccerDbContext dbContext, ICompanyUserRepository companyUserRepository, IFormOfPaymentRepository formOfPaymentRepository)
        {
            _paymentRepository = paymentRepository;
            _personCompanyRepository = personCompanyRepository;
            _companyUserRepository = companyUserRepository;
            _formOfPaymentRepository = formOfPaymentRepository;
            _dbContext = dbContext;
        }
        public async Task<Payment> CreateAsync(decimal value, Guid soccerPitchReservationId, Guid? personCompanyId, string note, int idFormOfPayment, long userId, long companyId)
        {
            if (personCompanyId.HasValue)
            {
                var personCompany = await _personCompanyRepository.GetAsync(personCompanyId.Value);
                if (personCompany == null)
                    throw new BussinessException("Cliente não encontrado.");
            }
            var companyUser = await _companyUserRepository.GetAsync(userId);
            if (companyUser == null)
                throw new BussinessException("Usuário não encontrado");
            if(companyUser.CompanyId != companyId)
                throw new BussinessException("Empresa inválida");

            var formOfPayment = await _formOfPaymentRepository.GetAsync(idFormOfPayment);
            if(formOfPayment == null)
                throw new BussinessException("Forma de pagamento não encontrada");
            var payment = new Payment
            {
                CompanyUserId = companyUser.Id,
                Note = note,
                PersonCompanyId = personCompanyId,
                SoccerPitchReservationId = soccerPitchReservationId,
                Value = value,
                CreatedDate = DateTime.UtcNow,
                CompanyId = companyId,
                FormOfPaymentId = formOfPayment.Id
            };
            await _paymentRepository.Create(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        public Task<List<Payment>> GetAsync(Guid soccerPitchReservationId)
        {
            return _paymentRepository.GetAsync(soccerPitchReservationId);
        }

        public async Task<Payment> UpdateAsync(long idPayment, decimal value, Guid? personCompanyId, string note, int formOfPaymentId)
        {
            if (personCompanyId.HasValue)
            {
                var personCompany = await _personCompanyRepository.GetAsync(personCompanyId.Value);
                if (personCompany == null)
                    throw new BussinessException("Cliente não encontrado.");
            }
            var payment = await _paymentRepository.GetAsync(idPayment);
            if (payment == null)
                throw new BussinessException("Pagamento não encontrado.");
            payment.Value = value;
            payment.Note = note;
            payment.PersonCompanyId = personCompanyId;
            payment.FormOfPaymentId = formOfPaymentId;
            await _personCompanyRepository.Edit(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }
    }
}
