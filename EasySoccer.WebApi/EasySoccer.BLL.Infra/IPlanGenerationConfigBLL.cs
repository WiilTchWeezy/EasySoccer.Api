using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IPlanGenerationConfigBLL
    {
        Task<List<PlanGenerationConfig>> GetAsync(long companyId, int page, int pageSize);
        Task<int> GetTotalAsync(long companyId);
        Task<PlanGenerationConfig> CreateAsync(string name, int intervalBetweenReservations, int limitType, int limitQuantity, long companyId);
        Task<PlanGenerationConfig> UpdateAsync(long planGenerationConfig, string name, int intervalBetweenReservations, int limitType, int limitQuantity);
    }
}
