namespace EasySoccer.WebApi.ApiRequests.Base
{
    public class GetBaseRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetBaseRequest()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
