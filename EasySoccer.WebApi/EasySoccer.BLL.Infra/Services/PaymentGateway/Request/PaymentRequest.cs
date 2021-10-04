using System;

namespace EasySoccer.BLL.Infra.Services.PaymentGateway.Request
{
    public class GatewayPaymentRequest
    {
        public string FinancialName { get; set; }

        public string FinancialDocument { get; set; }

        public string FinancialBirthDay { get; set; }

        public string CardNumber { get; set; }

        public string SecurityCode { get; set; }

        public string CardExpiration { get; set; }

        public int SelectedPlan { get; set; }

        public int SelectedInstallments { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }

        public string Complementary { get; set; }
    }
}
