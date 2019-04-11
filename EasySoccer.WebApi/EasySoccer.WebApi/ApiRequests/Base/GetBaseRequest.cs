namespace EasySoccer.WebApi.ApiRequests.Base
{
    public class GetBaseRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetBaseRequest()
        {

        }
    }
}
