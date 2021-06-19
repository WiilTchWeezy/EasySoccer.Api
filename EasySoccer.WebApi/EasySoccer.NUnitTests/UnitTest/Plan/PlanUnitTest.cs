using EasySoccer.BLL;
using EasySoccer.DAL;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySoccer.Test.UnitTest.Plan
{
    public class PlanUnitTest
    {
        private Mock<IEasySoccerDbContext> _dbContextMock;
        [SetUp]
        public void Setup()
        {
            var planDbSetMock = new Mock<DbSet<SoccerPitchPlan>>();
            var soccerPlanDbSetMock = new Mock<DbSet<SoccerPitchSoccerPitchPlan>>();
            var searches = new List<SoccerPitchPlan>();
            var query = searches.AsQueryable();
            planDbSetMock.As<IQueryable<SoccerPitchPlan>>().Setup(m => m.Provider).Returns(query.Provider);
            planDbSetMock.As<IQueryable<SoccerPitchPlan>>().Setup(m => m.Expression).Returns(query.Expression);
            planDbSetMock.As<IQueryable<SoccerPitchPlan>>().Setup(m => m.ElementType).Returns(query.ElementType);
            planDbSetMock.As<IQueryable<SoccerPitchPlan>>().Setup(m => m.GetEnumerator()).Returns(() => query.GetEnumerator());
            planDbSetMock.Setup(d => d.Add(It.IsAny<SoccerPitchPlan>())).Callback<SoccerPitchPlan>(searches.Add);
            _dbContextMock = new Mock<IEasySoccerDbContext>();
            _dbContextMock.Setup(x => x.SoccerPitchPlanQuery).Returns(planDbSetMock.Object.AsQueryable());
        }

        [Test]
        public void CreatePlan_Test()
        {
            var planBLL = new SoccerPitchPlanBLL(_dbContextMock.Object, new SoccerPitchPlanRepository(_dbContextMock.Object), new SoccerPitchSoccerPitchPlanRepository(_dbContextMock.Object));
            var plan = planBLL.CreateAsync("Mensal", 120, 1, "Ganhe uma Skoll").GetAwaiter().GetResult();
            Assert.IsNotNull(plan);
        }
    }
}
