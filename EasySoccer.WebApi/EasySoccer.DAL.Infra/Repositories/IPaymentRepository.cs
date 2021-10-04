using EasySoccer.Entities;
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
    }
}
