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
                var token = GenerateToken(new GenericIdentity(user.Email, "Email"), tokenConfigurations, signingConfigurations, new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
                    });

                return Ok(token);
            }
            else
            {
                return BadRequest(new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                });
            }
        }

        [AllowAnonymous]
        [Route("api/login/tokencompany"), HttpGet]
        public async Task<IActionResult> LoginCompanyAsync
    ([FromQuery]string email, [FromQuery]string password,
    [FromServices]TokenConfigurations tokenConfigurations,
    [FromServices]SigningConfigurations signingConfigurations)
        {
            var user = await _uow.CompanyUserBLL.LoginAsync(email, password);
            if (user != null)
            {
                var token = GenerateToken(new GenericIdentity(user.Email, "Email"), tokenConfigurations, signingConfigurations, new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.FamilyName, user.CompanyId.ToString())
                    });

                return Ok(token);
            }
            else
            {
                return BadRequest(new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                });
            }
        }


        private TokenResponse GenerateToken(IIdentity identityClaim, TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations, params Claim[] clains)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    identityClaim,
                    clains
                );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromMinutes(tokenConfigurations.Seconds);

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

            return new TokenResponse
            {
                Token = token,
                ExpireDate = dataExpiracao
            };
        }

    }
}
