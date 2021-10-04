using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;

namespace EasySoccer.WebApi.UoWs
{
    public class FormOfPaymentUoW : UoWBase
    {
        public IFormOfPaymentBLL FormOfPaymentBLL { get; set; }
        public FormOfPaymentUoW(IFormOfPaymentBLL formOfPaymentBLL, IEasySoccerDbContext dbContext) : base(dbContext)
        {
            FormOfPaymentBLL = formOfPaymentBLL;
        }
    }
}
