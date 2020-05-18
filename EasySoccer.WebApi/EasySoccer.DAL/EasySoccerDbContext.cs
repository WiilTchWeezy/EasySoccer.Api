using EasySoccer.DAL.Infra;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL
{
    public class EasySoccerDbContext : DbContext, IEasySoccerDbContext
    {
        public DbSet<User> User { get; set; }
        public IQueryable<User> UserQuery { get { return User; } }

        public DbSet<Company> Company { get; set; }
        public IQueryable<Company> CompanyQuery { get { return Company; } }

        public DbSet<CompanySchedule> CompanySchedule { get; set; }
        public IQueryable<CompanySchedule> CompanyScheduleQuery { get { return CompanySchedule; } }

        public DbSet<SoccerPitch> SoccerPitch { get; set; }
        public IQueryable<SoccerPitch> SoccerPitchQuery { get { return SoccerPitch; } }

        public DbSet<SoccerPitchPlan> SoccerPitchPlan { get; set; }
        public IQueryable<SoccerPitchPlan> SoccerPitchPlanQuery { get { return SoccerPitchPlan; } }

        public DbSet<SoccerPitchReservation> SoccerPitchReservation { get; set; }
        public IQueryable<SoccerPitchReservation> SoccerPitchReservationQuery { get { return SoccerPitchReservation; } }

        public DbSet<CompanyUser> CompanyUser { get; set; }
        public IQueryable<CompanyUser> CompanyUserQuery { get { return CompanyUser; } }

        public DbSet<SoccerPitchSoccerPitchPlan> SoccerPitchSoccerPitchPlan { get; set; }
        public IQueryable<SoccerPitchSoccerPitchPlan> SoccerPitchSoccerPitchPlanQuery { get { return SoccerPitchSoccerPitchPlan; } }

        public DbSet<SportType> SportType { get; set; }
        public IQueryable<SportType> SportTypeQuery { get { return SportType; } }

        #region Infra

        public EasySoccerDbContext(DbContextOptions op) : base(op)
        {

        }

        public Task Delete<TEntity>(TEntity entity) where TEntity : class
        {
            return Task.Run(() =>
            {
                base.Remove(entity);
            });
        }

        public Task Edit<TEntity>(TEntity entity) where TEntity : class
        {
            return Task.Run(() =>
            {
                base.Update(entity);
            });
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            return Task.Run(() =>
            {
                base.Add(entity);
            });
        } 
        #endregion
    }
}
