using EasySoccer.BLL;
using EasySoccer.BLL.Services.Azure;
using EasySoccer.BLL.Services.Cryptography;
using EasySoccer.DAL.Repositories;
using EasySoccer.Test.SetUp;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.Test.UnitTest.SoccerPitchReservation
{
    public class SoccerPitchReservationUnitTest
    {
        public SoccerPitchReservationBLL SoccerPitchReservationBLL { get; set; }
        public CompanyUserNotificationBLL CompanyUserNotificationBLL { get; set; }
        public SoccerPitchPlanBLL SoccerPitchPlanBLL { get; set; }
        public SoccerPitchBLL SoccerPitchBLL { get; set; }
        public CompanyBLL CompanyBLL { get; set; }

        [SetUp]
        public void SetUp()
        {
            var dbContext = DatabaseSetUp.Instance.SetUpContext();
            var config = ConfigSetUp.Instance.SetUpConfiguration();
            var notificationServiceMock = new Mock<BLL.Infra.Services.PushNotification.INotificationService>();
            var blobStorageServiceMock = new Mock<BLL.Infra.Services.Azure.IBlobStorageService>();

            CompanyUserNotificationBLL = new CompanyUserNotificationBLL(
                    notificationServiceMock.Object,
                    new CompanyUserNotificationRepository(dbContext),
                    dbContext,
                    new UserTokenRepository(dbContext)
                );

            SoccerPitchReservationBLL = new SoccerPitchReservationBLL(
                new SoccerPitchReservationRepository(dbContext),
                new SoccerPitchRepository(dbContext),
                dbContext,
                new SoccerPitchSoccerPitchPlanRepository(dbContext),
                new CompanyScheduleRepository(dbContext),
                new PersonRepository(dbContext),
                new UserRepository(dbContext),
                new UserTokenRepository(dbContext),
                new CompanyUserRepository(dbContext),
                CompanyUserNotificationBLL,
                new CompanyRepository(dbContext),
                new PersonCompanyRepository(dbContext)
                );

            SoccerPitchPlanBLL = new SoccerPitchPlanBLL(dbContext, new SoccerPitchPlanRepository(dbContext), new SoccerPitchSoccerPitchPlanRepository(dbContext));

            SoccerPitchBLL = new SoccerPitchBLL(new SoccerPitchRepository(dbContext), new SoccerPitchSoccerPitchPlanRepository(dbContext), dbContext, new SportTypeRepository(dbContext), blobStorageServiceMock.Object);
            var emailServiceMock = new Mock<BLL.Infra.Services.SendGrid.IEmailService>();

            CompanyBLL = new CompanyBLL(
                new CompanyRepository(dbContext),
                dbContext,
                new CompanyScheduleRepository(dbContext),
                new BlobStorageService(config),
                new FormInputRepository(dbContext),
                emailServiceMock.Object,
                new CompanyUserRepository(dbContext),
                config,
                new CompanyFinancialRecordRepository(dbContext),
                new CityRepository(dbContext),
                new StateRepository(dbContext),
                new CryptographyService(config),
                new CompanyUserNotificationRepository(dbContext));

        }

        [Test]
        public void InserReservationWebApp()
        {
            var request = new BLL.Infra.DTO.FormInputCompanyEntryRequest
            {
                CompanyName = "Complexo Esportivo Teste II",
                CompanyDocument = "84.207.476/0001-98",
                ConfirmPassword = "@123Teste",
                Password = "@123Teste",
                UserEmail = "teste2@testes.com",
                UserName = "Test User"
            };
            CompanyBLL.SaveFormInputCompanyAsync(request).GetAwaiter().GetResult();
            var company = CompanyBLL.GetAsync(request.CompanyDocument).GetAwaiter().GetResult();
            var soccerPitchPlan = SoccerPitchPlanBLL.CreateAsync("Plano Teste", 100, company.Id, "Descrição Teste").GetAwaiter().GetResult();
            if (soccerPitchPlan != null)
            {
                var soccerPitch = SoccerPitchBLL.CreateAsync("Quadra Teste", "Descrição Teste", false, 14, company.Id, true, new BLL.Infra.DTO.SoccerPitchPlanRequest[] { new BLL.Infra.DTO.SoccerPitchPlanRequest { Id = soccerPitchPlan.Id, IsDefault = true } }, 1, 60, "", "").GetAwaiter().GetResult();
                if(soccerPitch != null)
                {
                    var reservation = SoccerPitchReservationBLL.CreateAsync(soccerPitch.Id, null, DateTime.UtcNow.AddDays(2), new TimeSpan(20,00,00), new TimeSpan(21,00,00), null, null, soccerPitchPlan.Id, null, Entities.Enum.ApplicationEnum.WebApp).GetAwaiter().GetResult();
                    Assert.IsNotNull(reservation);
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
