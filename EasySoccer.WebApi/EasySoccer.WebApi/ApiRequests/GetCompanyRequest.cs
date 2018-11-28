using EasySoccer.WebApi.ApiRequests.Base;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetCompanyRequest : GetBaseRequest
    {
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Description { get; set; }
    }
}
