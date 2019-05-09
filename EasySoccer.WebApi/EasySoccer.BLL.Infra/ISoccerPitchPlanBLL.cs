using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchPlanBLL
    {
        Task<List<SoccerPitchPlan>> GetAsync(int companyId, int page, int pageSize);
        Task<SoccerPitchPlan> CreateAsync(string name, decimal value, long companyId);
        Task<SoccerPitchPlan> UpdateAsync(int id, string name, decimal value);
    }
}
