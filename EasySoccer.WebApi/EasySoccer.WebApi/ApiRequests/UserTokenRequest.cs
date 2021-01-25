using System;

namespace EasySoccer.WebApi.ApiRequests
{
    public class UserTokenRequest
    {
        public long CompanyUserId { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
