namespace EasySoccer.DAL.Infra.Repositories
{
    public abstract class RepositoryBase
    {
        protected IEasySoccerDbContext _dbContext;
        public RepositoryBase(IEasySoccerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
