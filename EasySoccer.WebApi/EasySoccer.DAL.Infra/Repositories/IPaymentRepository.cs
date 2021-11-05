using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IPaymentRepository : IRepositoryBase
    {
        Task<List<Payment>> GetAsync(Guid soccerPitchReservationId);
        Task<Payment> GetAsync(long idPayment);
        Task<List<Payment>> GetAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment, PaymentStatusEnum? status, string personCompanyName, int page, int pageSize);
        Task<int> GetTotalAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment);
        Task<List<Payment>> GetAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus);
        Task<decimal> GetTotalValueAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus);
    }
}
