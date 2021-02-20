using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class ChangeStatusRequest
    {
        public Guid ReservationId { get; set; }
        public int Status { get; set; }
    }
}
