using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
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
        private ISoccerPitchSoccerPitchPlanRepository _soccerPitchSoccerPitchPlanRepository;
        private IEasySoccerDbContext _dbContext;
        public SoccerPitchPlanBLL(IEasySoccerDbContext dbContext,ISoccerPitchPlanRepository soccerPitchPlanRepository, ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository)
        {
            _soccerPitchPlanRepository = soccerPitchPlanRepository;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
            _dbContext = dbContext;
        }

        public async Task<SoccerPitchPlan> CreateAsync(string name, decimal value, long companyId)
        {
            var soccerPitchPlan = new SoccerPitchPlan
            {
                Name = name,
                Value = value,
                CompanyId = companyId
            };
            await _soccerPitchPlanRepository.Create(soccerPitchPlan);
            await _dbContext.SaveChangesAsync();
            return soccerPitchPlan;
        }

        public Task<List<SoccerPitchPlan>> GetAsync(int companyId, int page, int pageSize)
        {
            return _soccerPitchPlanRepository.GetAsync(companyId, page, pageSize);
        }

        public Task<List<SoccerPitchPlan>> GetAsync(long soccerPitchId)
        {
            return _soccerPitchSoccerPitchPlanRepository.GetPlansAsync(soccerPitchId);
        }

        public async Task<SoccerPitchPlan> UpdateAsync(int id, string name, decimal value)
        {
            var soccerPitchPlan = await _soccerPitchPlanRepository.GetAsync(id);
            if (soccerPitchPlan == null)
                throw new NotFoundException(soccerPitchPlan, id);
            soccerPitchPlan.Name = name;
            soccerPitchPlan.Value = value;
            await _soccerPitchPlanRepository.Edit(soccerPitchPlan);
            await _dbContext.SaveChangesAsync();
            return soccerPitchPlan;
        }
    }
}
