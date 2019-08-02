using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class UserUoW : UoWBase
    {
        public IUserBLL UserBLL { get; set; }
        public UserUoW(IEasySoccerDbContext dbContext, IUserBLL userBLL) : base(dbContext)
        {
            UserBLL = userBLL;
        }
    }
}
