using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        protected IEasySoccerDbContext _dbContext;
        public RepositoryBase(IEasySoccerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Create<T>(T entity) where T : class
        {
            return Task.Run(() =>
            {
                _dbContext.Add(entity);
            });
        }

        public Task Edit<T>(T entity) where T : class
        {
            return Task.Run(() =>
            {
                _dbContext.Edit(entity);
            });
        }
    }
}
