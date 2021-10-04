using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IFormOfPaymentBLL
    {
        Task<FormOfPayment> CreateAsync(string name, bool active, long companyId);
        Task<FormOfPayment> UpdateAsync(int formOfPaymentId, string name, bool active);
        Task<List<FormOfPayment>> GetAsync(long companyId);
        Task<List<FormOfPayment>> GetAsync(int page, int pageSize, long companyId);
    }
}
