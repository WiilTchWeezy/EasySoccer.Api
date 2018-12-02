using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security;
using EasySoccer.WebApi.Security.Entity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.Controllers
{
    [Produces("application/json")]
    public class LoginController : ApiBaseController
    {
        private LoginUoW _uow;
        public LoginController(LoginUoW uow) : base(uow)
        {
            _uow = uow;
        }


        [AllowAnonymous]
        [Route("api/login/token"), HttpGet]
        public async Task<IActionResult> LoginAsync
            ([FromQuery]string email, [FromQuery]string password, 
            [FromServices]TokenConfigurations tokenConfigurations, 
            [FromServices]SigningConfigurations signingConfigurations)
        {
            var user = await _uow.UserBLL.LoginAsync(email, password);
            if (user != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Email, "Email"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return Ok( new
                {
                    accessToken = token
                });
            }
            else
            {
                return BadRequest (new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                });
            }
        }

    }
}
