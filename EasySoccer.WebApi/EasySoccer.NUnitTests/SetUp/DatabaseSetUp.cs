using EasySoccer.DAL;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.Test.SetUp
{
    public class DatabaseSetUp
    {
        private static DatabaseSetUp _intance;
        public static DatabaseSetUp Instance
        {
            get
            {
                if (_intance == null)
                    _intance = new DatabaseSetUp();
                return _intance;
            }
        }
        public EasySoccerDbContext SetUpContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EasySoccerDbContext>()
                .UseInMemoryDatabase("EasySoccerDb").Options;
            var context = new EasySoccerDbContext(dbContextOptions);
            return context;
        }

        public Task<List<Company>> SeedCompany()
        {
            using (var dbContext = SetUpContext())
            {
                dbContext.Company.Add(new Entities.Company 
                {
                    Active = true,
                    CNPJ = "87.273.274/0001-32",
                    CompleteAddress = "Rua Exemplo 100, Jd dos Exemplos - Ribeirão Preto",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Este é um exemplo",
                    IdCity = 4400,
                    InsertReservationConfirmed = false,
                    Name = "Complexo Esportivo Teste",
                    WorkOnHoliDays = false
                });
                dbContext.SaveChanges();
                return dbContext.Company.ToListAsync();
            }
        } 
    }
}
