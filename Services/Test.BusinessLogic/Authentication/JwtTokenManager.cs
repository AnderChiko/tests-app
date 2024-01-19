using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.BusinessLogic.Interfaces;

namespace Test.BusinessLogic.Authentication
{
    public class JwtTokenManager : IJwtTokenManager
    {

        private readonly IConfiguration _configuration;
        private readonly IUserManager _userManager;
        public JwtTokenManager(IConfiguration configuration, IUserManager userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> Authenticate(string username, string password)
        {
            //
            var usersData = await _userManager.Get();
            if (usersData == null || !usersData.Data.Any(x => x.Username.Equals(username) && x.Password.Equals(password)))
                return null;

            var key = _configuration.GetValue<string>("TokenProviderOptions:SecretPassword");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                 Subject = new ClaimsIdentity(new Claim[] { 
                     new Claim(ClaimTypes.NameIdentifier, username) 
                 }),
                 Expires = DateTime.UtcNow.AddMinutes(30),
                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}