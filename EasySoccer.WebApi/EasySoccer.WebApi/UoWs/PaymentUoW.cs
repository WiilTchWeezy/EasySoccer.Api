using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class PaymentUoW : UoWBase
    {
        public IPaymentBLL PaymentBLL { get; set; }
        public PaymentUoW(IEasySoccerDbContext dbContext, IPaymentBLL paymentBLL) : base(dbContext)
        {
            this.PaymentBLL = paymentBLL;
        }

    }
}
