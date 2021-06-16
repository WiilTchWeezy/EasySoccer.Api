using EasySoccer.BLL;
using EasySoccer.BLL.Services.Azure;
using EasySoccer.BLL.Services.Cryptography;
using EasySoccer.DAL.Repositories;
using EasySoccer.Test.SetUp;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Test.UnitTest.Company
{
    public class CompanyUnitTest
    {
        public CompanyBLL CompanyBLL { get; set; }
        [SetUp]
        public void SetUp()
        {
            var dbContext = DatabaseSetUp.Instance.SetUpContext();
            var config = ConfigSetUp.Instance.SetUpConfiguration();
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
        public void InsertCompanyTest()
        {
            var request = new BLL.Infra.DTO.FormInputCompanyEntryRequest
            {
                CompanyName = "Complexo Esportivo Teste",
                CompanyDocument = "07.720.233/0001-08",
                ConfirmPassword = "@123Teste",
                Password = "@123Teste",
                UserEmail = "teste@testes.com",
                UserName = "Test User"
            };
            CompanyBLL.SaveFormInputCompanyAsync(request).GetAwaiter().GetResult();
            var company = CompanyBLL.GetAsync(request.CompanyDocument).GetAwaiter().GetResult();
            Assert.IsNotNull(company);
        }
    }
}
