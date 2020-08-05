using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class SoccerPitchReservationRequest
    {
        public Guid Id { get; set; }
        public long SoccerPitchId { get; set; }
        public Guid UserId { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan HourStart { get; set; }
        public TimeSpan HourEnd { get; set; }
        public int SoccerPitchSoccerPitchPlanId { get; set; }
        public string Note { get; set; }
        public int SoccerPitchPlanId { get; set; }
    }
}
