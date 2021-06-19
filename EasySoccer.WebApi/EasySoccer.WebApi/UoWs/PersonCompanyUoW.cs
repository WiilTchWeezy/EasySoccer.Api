using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class PersonCompanyUoW : UoWBase
    {
        public IPersonCompanyBLL PersonCompanyBLL { get; set; }
        public PersonCompanyUoW(IEasySoccerDbContext dbContext, IPersonCompanyBLL personCompanyBLL) : base(dbContext)
        {
            PersonCompanyBLL = personCompanyBLL;
        }

    }
}
