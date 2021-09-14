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
    public class PlanGenerationConfigBLL : IPlanGenerationConfigBLL
    {
        private IEasySoccerDbContext _dbContext;
        private IPlanGenerationConfigRepository _planGenerationConfigRepository;
        public PlanGenerationConfigBLL(IEasySoccerDbContext dbContext, IPlanGenerationConfigRepository planGenerationConfigRepository)
        {
            _dbContext = dbContext;
            _planGenerationConfigRepository = planGenerationConfigRepository;
        }

        public async Task<PlanGenerationConfig> CreateAsync(string name, int intervalBetweenReservations, int limitType, int limitQuantity, long companyId)
        {
            var planGenerationConfig = new PlanGenerationConfig
            {
                CreatedDate = DateTime.UtcNow,
                IntervalBetweenReservations = intervalBetweenReservations,
                LimitQuantity = limitQuantity,
                Name = name,
                LimitType = (Entities.Enum.LimitTypeEnum)limitType,
                CompanyId = companyId
            };
            await _planGenerationConfigRepository.Create(planGenerationConfig);
            await _dbContext.SaveChangesAsync();
            return planGenerationConfig;
        }

        public Task<List<PlanGenerationConfig>> GetAsync(long companyId, int page, int pageSize)
        {
            return _planGenerationConfigRepository.GetAsync(companyId, page, pageSize);
        }

        public Task<int> GetTotalAsync(long companyId)
        {
            return _planGenerationConfigRepository.GetTotalAsync(companyId);
        }

        public async Task<PlanGenerationConfig> UpdateAsync(long planGenerationConfig, string name, int intervalBetweenReservations, int limitType, int limitQuantity)
        {
            var currenConfig = await _planGenerationConfigRepository.GetAsync(planGenerationConfig);
            if (currenConfig == null)
                throw new BussinessException("Configuração não encontrada!");
            currenConfig.IntervalBetweenReservations = intervalBetweenReservations;
            currenConfig.LimitQuantity = limitQuantity;
            currenConfig.LimitType = (Entities.Enum.LimitTypeEnum)limitType;
            currenConfig.Name = name;
            await _planGenerationConfigRepository.Edit(currenConfig);
            await _dbContext.SaveChangesAsync();
            return currenConfig;
        }
    }
}
