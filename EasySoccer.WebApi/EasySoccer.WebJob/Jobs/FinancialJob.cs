using EasySoccer.BLL.Infra.Services.PushNotification;
using EasySoccer.DAL;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using EasySoccer.WebJob.Jobs.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.WebJob.Jobs
{
    public class FinancialJob : IFinancialJob
    {
        private ICompanyFinancialRecordRepository _companyFinancialRecordRepository;
        private ICompanyUserNotificationRepository _companyUserNotificationRepository;
        private ICompanyUserRepository _companyUserRepository;
        private IEasySoccerDbContext _dbContext;
        private INotificationService _notificationService;
        private IUserTokenRepository _userTokenRepository;
        public FinancialJob(
            ICompanyFinancialRecordRepository companyFinancialRecordRepository,
            IEasySoccerDbContext dbContext, 
            ICompanyUserNotificationRepository companyUserNotificationRepository,
            ICompanyUserRepository companyUserRepository,
            INotificationService notificationService,
            IUserTokenRepository userTokenRepository)
        {
            _companyFinancialRecordRepository = companyFinancialRecordRepository;
            _dbContext = dbContext;
            _companyUserNotificationRepository = companyUserNotificationRepository;
            _companyUserRepository = companyUserRepository;
            _notificationService = notificationService;
            _userTokenRepository = userTokenRepository;
        }
        public async Task GenerateNotificationsToFinancialRecords()
        {
            Console.WriteLine(string.Format("{0} - Iniciando Jobs de registros financeiros atrasados/para atrasar.", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")));
            var companies = await _companyFinancialRecordRepository.GetCompaniesToGetLate();
            if (companies != null && companies.Any())
            {
                Console.WriteLine(string.Format("{0} - {1} registros encontrados.", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"), companies.Count));
                foreach (var item in companies)
                {
                    var companyUsers = await _companyUserRepository.GetByCompanyIdAsync(item.CompanyId);
                    foreach (var user in companyUsers)
                    {
                        var companyUserNotification = await _companyUserNotificationRepository.GetAsync(user.Id, DateTime.UtcNow.AddDays(-7));
                        if (companyUserNotification == null)
                        {
                            Console.WriteLine(string.Format("{0} - Inserindo notificação para usuário {1} - Empresa: {2}.", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"), user.Name, item.Company.Name));
                            companyUserNotification = new CompanyUserNotification
                            {
                                CreatedDate = DateTime.UtcNow,
                                Id = Guid.NewGuid(),
                                IdCompanyUser = user.Id,
                                Message = "Seu contrato está chegando no vencimento. Renove agora, para continuar com os beneficios da nossa plataforma.",
                                NotificationType = NotificationTypeEnum.FinancialRenewal,
                                Read = false,
                                Title = "Seu contrato está chegando no vencimento"
                            };
                            await _companyUserNotificationRepository.Create(companyUserNotification);
                        }
                        var companyUserTokens = await _userTokenRepository.GetAsync(user.Id);
                        if (companyUserTokens != null)
                        {
                            foreach (var userToken in companyUserTokens)
                            {
                                Console.WriteLine(string.Format("{0} - Notificando usuário.", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")));
                                var dic = new Dictionary<string, string>();
                                dic.Add("title", companyUserNotification.Title);
                                dic.Add("message", companyUserNotification.Message);
                                await _notificationService.SendNotification(userToken.Token, dic);
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            Console.WriteLine(string.Format("{0} - Finalizando Jobs de registros financeiros atrasados/para atrasar.", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")));
        }
    }
}
