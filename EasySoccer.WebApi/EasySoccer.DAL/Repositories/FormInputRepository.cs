using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;

namespace EasySoccer.DAL.Repositories
{
    public class FormInputRepository : RepositoryBase, IFormInputRepository
    {
        public FormInputRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
