using EasySoccer.Entities.Enum;
using EasySoccer.WebApi.ApiRequests.Base;
using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetSoccerPitchReservationRequest : GetBaseRequest
    {
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public int? SoccerPitchId { get; set; }
        public int? SoccerPitchPlanId { get; set; }
        public string UserName { get; set; }

        public string Status { get; set; }
    }
}
