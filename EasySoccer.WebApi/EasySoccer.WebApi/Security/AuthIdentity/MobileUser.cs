using EasySoccer.WebApi.Security.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Security.AuthIdentity
{
    public class MobileUser
    {

        public Guid UserId { get; set; }
        public Guid PersonId { get; set; }

        public ProfilesEnum Profile { get; set; }

        public MobileUser(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userId = claims.Where(x => x.Type == "UserId").FirstOrDefault()?.Value;
                string personId = claims.Where(x => x.Type == "PersonId").FirstOrDefault()?.Value;
                string profile = claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender").FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(profile))
                {
                    if (profile.Equals("User"))
                    {
                        Profile = ProfilesEnum.User;
                        if (string.IsNullOrEmpty(userId) == false)
                            UserId = Guid.Parse(userId);

                        if (string.IsNullOrEmpty(personId) == false)
                            PersonId = Guid.Parse(personId);
                    }
                }
            }
        }
    }
}
