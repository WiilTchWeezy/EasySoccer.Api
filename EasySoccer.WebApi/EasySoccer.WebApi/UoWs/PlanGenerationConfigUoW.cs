using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.UoWs
{
    public class PlanGenerationConfigUoW : UoWBase
    {
        public IPlanGenerationConfigBLL PlanGenerationConfigBLL { get; set; }
        public PlanGenerationConfigUoW(IEasySoccerDbContext dbContext, IPlanGenerationConfigBLL planGenerationConfigBLL) : base(dbContext)
        {
            PlanGenerationConfigBLL = planGenerationConfigBLL;
        }
    }
}
