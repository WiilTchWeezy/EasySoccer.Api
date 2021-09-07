using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.ApiRequests
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
