using EasySoccer.WebApi.Security.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Security.AuthIdentity
{
    public class CurrentUser
    {
        public long CompanyId { get; set; }

        public long UserId { get; set; }

        public ProfilesEnum Profile { get; set; }

        public CurrentUser(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userId = claims.Where(x => x.Type == "jti").FirstOrDefault()?.Value;
                string profile = claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender").FirstOrDefault()?.Value;
                string companyId = claims.Where(x => x.Type == "CompanyId").FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(profile))
                {
                    if (profile.Equals("CompanyUser"))
                    {
                        Profile = ProfilesEnum.CompanyUser;
                        if (string.IsNullOrEmpty(userId) == false)
                            UserId = long.Parse(userId);
                        if (string.IsNullOrEmpty(companyId) == false)
                            CompanyId = long.Parse(companyId);
                    }
                }
            }
        }
    }
}
