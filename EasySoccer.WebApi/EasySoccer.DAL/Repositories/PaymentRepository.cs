using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
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
            return _dbContext.PaymentQuery.Include(x => x.PersonCompany).Where(x => x.SoccerPitchReservationId == soccerPitchReservationId).OrderBy(x => x.CreatedDate).ToListAsync();
        }

        public Task<Payment> GetAsync(long idPayment)
        {
            return _dbContext.PaymentQuery.Where(x => x.Id == idPayment).FirstOrDefaultAsync();
        }
    }
}
