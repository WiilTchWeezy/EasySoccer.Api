namespace EasySoccer.WebApi.ApiRequests
{
    public class FormOfPaymentRequest
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int FormOfPaymentId { get; set; }
    }
}
