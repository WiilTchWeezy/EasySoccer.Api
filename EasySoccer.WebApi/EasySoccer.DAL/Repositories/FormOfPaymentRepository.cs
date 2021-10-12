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
    public class FormOfPaymentRepository : RepositoryBase, IFormOfPaymentRepository
    {
        public FormOfPaymentRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<FormOfPayment>> GetAsync(long companyId)
        {
            return _dbContext.FormOfPaymentQuery.OrderBy(x => x.Name).Where(x => x.CompanyId == companyId).ToListAsync();
        }

        public Task<List<FormOfPayment>> GetAsync(long companyId, int page, int pageSize)
        {
            return _dbContext.FormOfPaymentQuery.OrderBy(x => x.Name).Where(x => x.CompanyId == companyId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();            
        }

        public Task<FormOfPayment> GetAsync(int idFormOfPayment)
        {
            return _dbContext.FormOfPaymentQuery.Where(x => x.Id == idFormOfPayment).FirstOrDefaultAsync();
        }

        public Task<int> GetTotalAsync(long companyId)
        {
            return _dbContext.FormOfPaymentQuery.OrderBy(x => x.Name).Where(x => x.CompanyId == companyId).CountAsync();
        }
    }
}
