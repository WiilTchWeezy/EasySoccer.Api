using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class LoginUoW : UoWBase
    {
        public IUserBLL UserBLL { get; set; }
        public LoginUoW(IEasySoccerDbContext dbContext, IUserBLL userBLL) : base(dbContext)
        {
            UserBLL = userBLL;
        }
    }
}
