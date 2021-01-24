using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace EasySoccer.WebApi.Security
{
    public class SigningConfigurations
    {
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("TokenConfigurations");
            var key = configSection.GetValue<string>("TokenSecret");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
