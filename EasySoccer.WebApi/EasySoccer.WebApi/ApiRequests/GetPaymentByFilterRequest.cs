using EasySoccer.WebApi.ApiRequests.Base;
using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetPaymentByFilterRequest : GetBaseRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? FormOfPayment { get; set; }
        public int? Status { get; set; }
        public string PersonCompanyName { get; set; }
    }
}
