using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class PaymentRepository : RepositoryBase, IPaymentRepository
    {
        public PaymentRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {

        }

        public Task<List<Payment>> GetAsync(Guid soccerPitchReservationId)
        {
            return _dbContext.PaymentQuery.Include(x => x.PersonCompany).Include(x => x.FormOfPayment).Where(x => x.SoccerPitchReservationId == soccerPitchReservationId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public Task<Payment> GetAsync(long idPayment)
        {
            return _dbContext.PaymentQuery.Where(x => x.Id == idPayment).FirstOrDefaultAsync();
        }

        public Task<List<Payment>> GetAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment, int page, int pageSize)
        {
            var query = _dbContext.PaymentQuery;
            if (startDate.HasValue)
                query = query.Where(x => startDate.Value >= x.CreatedDate);
            if(endDate.HasValue)
                query = query.Where(x => endDate.Value <= x.CreatedDate);
            if(formOfPayment.HasValue)
                query = query.Where(x => x.FormOfPaymentId == formOfPayment);

            return query.Include(x => x.FormOfPayment).Include(x => x.PersonCompany).Include(x => x.SoccerPitchReservation)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<List<Payment>> GetAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus)
        {
            var query = _dbContext.PaymentQuery.Include(x => x.PersonCompany).Include(x => x.FormOfPayment).Where(x => x.SoccerPitchReservationId == soccerPitchReservationId);
            if (paymentStatus.HasValue)
                query = query.Where(x => x.Status == paymentStatus.Value);
            return query.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public Task<int> GetTotalAsync(DateTime? startDate, DateTime? endDate, int? formOfPayment)
        {
            var query = _dbContext.PaymentQuery;
            if (startDate.HasValue)
                query = query.Where(x => startDate.Value >= x.CreatedDate);
            if (endDate.HasValue)
                query = query.Where(x => endDate.Value <= x.CreatedDate);
            if (formOfPayment.HasValue)
                query = query.Where(x => x.FormOfPaymentId == formOfPayment);

            return query.CountAsync();
        }

        public Task<decimal> GetTotalValueAsync(Guid soccerPitchReservationId, PaymentStatusEnum? paymentStatus)
        {
            var query = _dbContext.PaymentQuery.Where(x => x.SoccerPitchReservationId == soccerPitchReservationId);
            if (paymentStatus.HasValue)
                query = query.Where(x => x.Status == paymentStatus.Value);
            return query.SumAsync(x => x.Value);
        }
    }
}
