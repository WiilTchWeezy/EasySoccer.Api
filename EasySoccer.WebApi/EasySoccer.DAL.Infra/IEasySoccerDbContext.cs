using EasySoccer.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra
{
    public interface IEasySoccerDbContext : IDisposable
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
        IQueryable<SoccerPitchPlan> SoccerPitchPlanQuery { get; }
        IQueryable<SoccerPitchReservation> SoccerPitchReservationQuery { get; }
        IQueryable<CompanyUser> CompanyUserQuery { get; }
        IQueryable<SoccerPitchSoccerPitchPlan> SoccerPitchSoccerPitchPlanQuery { get; }
        IQueryable<SportType> SportTypeQuery { get; }
        IQueryable<FormInput> FormInputQuery { get; }
        IQueryable<CompanyFinancialRecord> CompanyFinancialRecordQuery { get; }
        IQueryable<Person> PersonQuery { get; }
        IQueryable<State> StateQuery { get; }
        IQueryable<City> CityQuery { get; }
        IQueryable<UserToken> UserTokenQuery { get; }

    }
}
