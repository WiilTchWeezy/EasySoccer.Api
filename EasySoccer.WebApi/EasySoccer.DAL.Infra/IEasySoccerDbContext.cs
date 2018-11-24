using EasySoccer.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra
{
    public interface IEasySoccerDbContext
    {
        #region Infra
        Task<int> SaveChangesAsync();
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        Task Edit<TEntity>(TEntity entity) where TEntity : class;
        Task Delete<TEntity>(TEntity entity) where TEntity : class;
        #endregion

        IQueryable<User> UserQuery { get; }
        IQueryable<Company> CompanyQuery { get; }
        IQueryable<CompanySchedule> CompanyScheduleQuery { get; }
        IQueryable<SoccerPitch> SoccerPitchQuery { get; }
        IQueryable<SoccerPitchPictures> SoccerPitchPicturesQuery { get; }
        IQueryable<SoccerPitchPlan> SoccerPitchPlanQuery { get; }
        IQueryable<SoccerPitchReservation> SoccerPitchReservationQuery { get; }
    }
}
