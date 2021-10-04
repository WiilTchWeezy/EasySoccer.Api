using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class PaymentRequest
    {
        public decimal Value { get; set; }
        public Guid SoccerPitchReservationId { get; set; }
        public Guid? PersonCompanyId { get; set; }
        public string Note { get; set; }
        public long PaymentId { get; set; }
        public int FormOfPaymentId { get; set; }
    }
}
