using EasySoccer.BLL.Exceptions;
using EasySoccer.WebApi.ApiRequests;
using EasySoccer.WebApi.Controllers.Base;
using EasySoccer.WebApi.Security;
using EasySoccer.WebApi.Security.Entity;
using EasySoccer.WebApi.Security.Enums;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
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
                var token = GenerateToken(new GenericIdentity(user.Email, "Email"), tokenConfigurations, signingConfigurations, false,new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                        new Claim (JwtRegisteredClaimNames.Gender, ProfilesEnum.User.ToString()),
                        new Claim("UserId", user.Id.ToString())
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
        [Route("api/login/tokenfromfacebook"), HttpGet]
        public async Task<IActionResult> LoginFromFacebookAsync
        ([FromQuery]FacebookLoginRequest request,
        [FromServices]TokenConfigurations tokenConfigurations,
        [FromServices]SigningConfigurations signingConfigurations)
        {
            try
            {
                var user = await _uow.UserBLL.LoginFromFacebookAsync(request.Email, request.Id, $"{request.First_name} {request.Last_name}", request.Birthday);
                if (user != null)
                {
                    var token = GenerateToken(new GenericIdentity(user.Email, "Email"), tokenConfigurations, signingConfigurations, false, new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                        new Claim (JwtRegisteredClaimNames.Gender, ProfilesEnum.User.ToString()),
                        new Claim("UserId", user.Id.ToString())
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
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("api/login/tokencompany"), HttpGet]
        public async Task<IActionResult> LoginCompanyAsync
    ([FromQuery]string email, [FromQuery]string password,
    [FromServices]TokenConfigurations tokenConfigurations,
    [FromServices]SigningConfigurations signingConfigurations)
        {
            try
            {
                var user = await _uow.CompanyUserBLL.LoginAsync(email, password);
                if (user != null)
                {
                    var token = GenerateToken(new GenericIdentity(user.Email, "Email"), tokenConfigurations, signingConfigurations, true, new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                        new Claim (JwtRegisteredClaimNames.Gender, ProfilesEnum.CompanyUser.ToString()),
                        new Claim ("CompanyId", user.CompanyId.ToString())
                    });

                    return Ok(token);
                }
                else
                {
                    return BadRequest(new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar - Usuário e/ou senha inválidos"
                    });
                }
            }
            catch (BussinessException be)
            {
                return BadRequest(new
                {
                    authenticated = false,
                    message = be.Message
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        private TokenResponse GenerateToken(IIdentity identityClaim, TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations, bool fromCompany, params Claim[] clains)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    identityClaim,
                    clains
                );

            DateTime dataCriacao = DateTime.Now;

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = fromCompany ? (DateTime?)DateTime.UtcNow.AddHours(48) : null
            });
            var token = handler.WriteToken(securityToken);

            return new TokenResponse
            {
                Token = token,
                ExpireDate = DateTime.UtcNow.AddHours(48)
            };
        }

    }
}
