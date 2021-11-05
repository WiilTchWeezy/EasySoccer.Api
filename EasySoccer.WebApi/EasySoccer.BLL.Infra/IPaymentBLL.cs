using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IPaymentBLL
    {
        Task<Payment> CreateAsync(decimal value, Guid soccerPitchReservationId, Guid? personCompanyId, string note, int idFormOfPayment,long userId, long companyId);
        Task<Payment> UpdateAsync(long idPayment, decimal value, Guid? personCompanyId, string note, int formOfPaymentId);
        Task<List<Payment>> GetAsync(Guid soccerPitchReservationId);
        Task<List<Payment>> GetAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus);
        Task<decimal> GetTotalValueAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus);
        Task<List<Payment>> GetAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment, PaymentStatusEnum? status, string personCompanyName, int page, int pageSize);
        Task<int> GetTotalAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment);
        Task<Payment> CancelAsync(long idPayment, long idCompany);
    }
}
