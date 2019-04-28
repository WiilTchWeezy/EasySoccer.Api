using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class SoccerPitchPlanBLL : ISoccerPitchPlanBLL
    {
        private ISoccerPitchPlanRepository _soccerPitchPlanRepository;
        public SoccerPitchPlanBLL(ISoccerPitchPlanRepository soccerPitchPlanRepository)
        {
            _soccerPitchPlanRepository = soccerPitchPlanRepository;
        }

        public async Task<SoccerPitchPlan> CreateAsync(string name, decimal value)
        {
            var soccerPitchPlan = new SoccerPitchPlan { };
            await _soccerPitchPlanRepository.Create(soccerPitchPlan);
            return soccerPitchPlan;
        }

        public Task<List<SoccerPitchPlan>> GetAsync(int companyId, int page, int pageSize)
        {
            return _soccerPitchPlanRepository.GetAsync(companyId, page, pageSize);
        }

        public async Task<SoccerPitchPlan> UpdateAsync(int id, string name, decimal value)
        {
            var soccerPitchPlan = await _soccerPitchPlanRepository.GetAsync(id);
            soccerPitchPlan.Name = name;
            soccerPitchPlan.Value = value;
            return soccerPitchPlan;
        }
    }
}
