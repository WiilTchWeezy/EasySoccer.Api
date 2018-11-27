using EasySoccer.DAL.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.UoWs
{
    public class UoWBase : IDisposable
    {
        private IEasySoccerDbContext _dbContext;
        public UoWBase(IEasySoccerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
