using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IFormOfPaymentRepository : IRepositoryBase
    {
        Task<List<FormOfPayment>> GetAsync(long companyId);
        Task<List<FormOfPayment>> GetAsync(long companyId, int page, int pageSize);
        Task<FormOfPayment> GetAsync(int idFormOfPayment);
    }
}
