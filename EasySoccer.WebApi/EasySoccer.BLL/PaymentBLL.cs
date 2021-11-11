using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
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
        private ISoccerPitchReservationRepository _soccerPitchReservationRepository;
        private ISoccerPitchReservationBLL _soccerPitchReservationBLL;
        public PaymentBLL
            (
            IPaymentRepository paymentRepository,
            IPersonCompanyRepository personCompanyRepository,
            IEasySoccerDbContext dbContext,
            ICompanyUserRepository companyUserRepository,
            IFormOfPaymentRepository formOfPaymentRepository,
            ISoccerPitchReservationRepository soccerPitchReservationRepository,
            ISoccerPitchReservationBLL soccerPitchReservationBLL
            )
        {
            _paymentRepository = paymentRepository;
            _personCompanyRepository = personCompanyRepository;
            _companyUserRepository = companyUserRepository;
            _formOfPaymentRepository = formOfPaymentRepository;
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _dbContext = dbContext;
            _soccerPitchReservationBLL = soccerPitchReservationBLL;
        }

        public async Task<Payment> CancelAsync(long idPayment, long idCompany)
        {
            var payment = await _paymentRepository.GetAsync(idPayment);
            if (payment == null)
                throw new BussinessException("Pagamento não encontrado.");
            if (payment.CompanyId != idCompany)
                throw new BussinessException("Pagamento não pertence a sua empresa.");
            payment.Status = Entities.Enum.PaymentStatusEnum.Canceled;
            await _paymentRepository.Edit(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
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
            if (companyUser.CompanyId != companyId)
                throw new BussinessException("Empresa inválida");

            var formOfPayment = await _formOfPaymentRepository.GetAsync(idFormOfPayment);
            if (formOfPayment == null)
                throw new BussinessException("Forma de pagamento não encontrada");
            var reservation = await _soccerPitchReservationRepository.GetAsync(soccerPitchReservationId);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada");
            Guid reservationId = reservation.Id;
            if (reservation.OringinReservationId.HasValue)
            {
                var originReservation = await _soccerPitchReservationRepository.GetAsync(reservation.OringinReservationId.Value);
                if (originReservation == null)
                    throw new BussinessException("Reserva não encontrada");
                reservationId = originReservation.Id;
            }

            var payment = new Payment
            {
                CompanyUserId = companyUser.Id,
                Note = note,
                PersonCompanyId = personCompanyId,
                SoccerPitchReservationId = reservationId,
                Value = value,
                CreatedDate = DateTime.UtcNow,
                CompanyId = companyId,
                FormOfPaymentId = formOfPayment.Id,
                Status = Entities.Enum.PaymentStatusEnum.Created
            };
            await _paymentRepository.Create(payment);
            await _dbContext.SaveChangesAsync();
            var reservationValue = await _soccerPitchReservationBLL.GetReservationValueAsync(reservationId);
            var totalPayments = await GetTotalValueAsync(reservationId, PaymentStatusEnum.Created);
            if(totalPayments >= reservationValue)
            {
                await _soccerPitchReservationBLL.ChangeStatusAllReservationsAsync(reservationId);
            }
            return payment;
        }

        public async Task<List<Payment>> GetAsync(Guid soccerPitchReservationId)
        {
            var reservation = await _soccerPitchReservationRepository.GetAsync(soccerPitchReservationId);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada");
            Guid reservationId = reservation.Id;
            if (reservation.OringinReservationId.HasValue)
                reservationId = reservation.OringinReservationId.Value;
            return await _paymentRepository.GetAsync(reservationId);
        }

        public Task<List<Payment>> GetAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment, PaymentStatusEnum? status, string personCompanyName, int page, int pageSize)
        {
            return _paymentRepository.GetAsync(startDate, endDate, formOfPayment, status, personCompanyName, page, pageSize);
        }

        public async Task<List<Payment>> GetAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus)
        {
            var reservation = await _soccerPitchReservationRepository.GetAsync(soccerPitchReservationId);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada");
            Guid reservationId = reservation.Id;
            if (reservation.OringinReservationId.HasValue)
                reservationId = reservation.OringinReservationId.Value;
            return await _paymentRepository.GetAsync(reservationId, paymentStatus);
        }

        public Task<int> GetTotalAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment)
        {
            return _paymentRepository.GetTotalAsync(startDate, endDate, formOfPayment);
        }

        public async Task<decimal> GetTotalValueAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus)
        {
            var reservation = await _soccerPitchReservationRepository.GetAsync(soccerPitchReservationId);
            if (reservation == null)
                throw new BussinessException("Reserva não encontrada");
            Guid reservationId = reservation.Id;
            if (reservation.OringinReservationId.HasValue)
                reservationId = reservation.OringinReservationId.Value;
            return await _paymentRepository.GetTotalValueAsync(reservationId, paymentStatus);

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
