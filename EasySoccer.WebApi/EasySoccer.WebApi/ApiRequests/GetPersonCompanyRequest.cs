using EasySoccer.WebApi.ApiRequests.Base;

namespace EasySoccer.WebApi.ApiRequests
{
    public class GetPersonCompanyRequest : GetBaseRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
