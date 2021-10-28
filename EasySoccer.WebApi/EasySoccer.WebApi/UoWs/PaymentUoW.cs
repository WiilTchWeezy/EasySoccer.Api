using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class PaymentUoW : UoWBase
    {
        public IPaymentBLL PaymentBLL { get; set; }
        public ISoccerPitchReservationBLL SoccerPitchReservationBLL { get; set; }
        public PaymentUoW(IEasySoccerDbContext dbContext, IPaymentBLL paymentBLL, ISoccerPitchReservationBLL soccerPitchReservationBLL) : base(dbContext)
        {
            this.PaymentBLL = paymentBLL;
            SoccerPitchReservationBLL = soccerPitchReservationBLL;
        }

    }
}
