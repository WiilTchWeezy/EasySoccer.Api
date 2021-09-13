using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IPlanGenerationConfigBLL
    {
        Task<List<PlanGenerationConfig>> GetAsync(int page, int pageSize);
        Task<PlanGenerationConfig> CreateAsync(string name, int intervalBetweenReservations, int limitType, int limitQuantity);
        Task<PlanGenerationConfig> UpdateAsync(long planGenerationConfig, string name, int intervalBetweenReservations, int limitType, int limitQuantity);
    }
}
