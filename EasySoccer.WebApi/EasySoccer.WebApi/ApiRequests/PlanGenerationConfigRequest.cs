using EasySoccer.Entities.Enum;

namespace EasySoccer.WebApi.ApiRequests
{
    public class PlanGenerationConfigRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int IntervalBetweenReservations { get; set; }
        public int LimitType { get; set; }
        public int LimitQuantity { get; set; }
    }
}
