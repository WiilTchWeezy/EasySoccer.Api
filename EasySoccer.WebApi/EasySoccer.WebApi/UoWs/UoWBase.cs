using EasySoccer.DAL.Infra;
using System;

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
