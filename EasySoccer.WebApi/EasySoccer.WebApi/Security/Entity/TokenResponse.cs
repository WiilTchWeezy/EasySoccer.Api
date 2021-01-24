using System;

namespace EasySoccer.WebApi.Security.Entity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
