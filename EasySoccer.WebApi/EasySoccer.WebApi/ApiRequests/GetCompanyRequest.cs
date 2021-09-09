using EasySoccer.WebApi.ApiRequests.Base;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetCompanyRequest : GetBaseRequest
    {
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }

        public string OrderField { get; set; }

        public string OrderDirection { get; set; }
        public int IdCity { get; set; }
        public int IdState { get; set; }
    }
}
