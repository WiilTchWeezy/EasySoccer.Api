using EasySoccer.WebApi.ApiRequests.Base;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetSoccerPitchRequest : GetBaseRequest
    {
        public int CompanyId { get; set; }
    }
}
