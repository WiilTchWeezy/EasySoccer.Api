using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IPlanGenerationConfigRepository : IRepositoryBase
    {
        Task<PlanGenerationConfig> GetAsync(long id);
        Task<List<PlanGenerationConfig>> GetAsync(int page, int pageSize);
    }
}
