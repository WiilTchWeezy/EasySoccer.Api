﻿namespace EasySoccer.WebApi.ApiRequests
{
    public class PaymentRequest
    {
        public string FinancialName { get; set; }

        public string FinancialDocument { get; set; }

        public string FinancialBirthDay { get; set; }

        public string CardNumber { get; set; }

        public string SecurityCode { get; set; }

        public string CardExpiration { get; set; }

        public int SelectedPlan { get; set; }

        public int SelectedInstallments { get; set; }
    }
}
