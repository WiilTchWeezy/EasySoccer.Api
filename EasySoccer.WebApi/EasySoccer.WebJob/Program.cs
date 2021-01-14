using EasySoccer.BLL.Infra.Services.PushNotification;
using EasySoccer.BLL.Services.PushNotification;
using EasySoccer.DAL;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.DAL.Repositories;
using EasySoccer.WebJob.Jobs;
using EasySoccer.WebJob.Jobs.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EasySoccer.WebJob
{
    class Program
    {
        public static IConfiguration Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                                .AddJsonFile("appsettings.json", optional: true);
                return builder.Build();
            }
        }
        static void Main(string[] args)
        {
            var serviceCollector = new ServiceCollection();
            ConfigureServices(serviceCollector);
            var serviceProvider = serviceCollector.BuildServiceProvider();

            #region JobsStart
            try
            {
                var financialJob = serviceProvider.GetService<IFinancialJob>();
                financialJob.GenerateNotificationsToFinancialRecords().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Erro no serviço de notificação - {0}", e.Message));
            }
            #endregion
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            #region Jobs
            services
                .AddScoped<IFinancialJob, FinancialJob>()
                .AddScoped<ICompanyUserNotificationRepository, CompanyUserNotificationRepository>()
                .AddScoped<ICompanyFinancialRecordRepository, CompanyFinancialRecordRepository>()
                .AddScoped<ICompanyUserRepository, CompanyUserRepository>()
                .AddScoped<IUserTokenRepository, UserTokenRepository>()
                .AddScoped<INotificationService, NotificationService>()
                .AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<IEasySoccerDbContext, EasySoccerDbContext>(
                        x => x.UseSqlServer(Configuration.GetConnectionString("EasySoccerDbContext")));
            #endregion
        }
    }
}
