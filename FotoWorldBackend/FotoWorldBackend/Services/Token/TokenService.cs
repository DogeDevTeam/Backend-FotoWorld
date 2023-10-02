using FotoWorldBackend.Models;
using FotoWorldBackend.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FotoWorldBackend.Services.Token
{
    /// <summary>
    /// Provides JWT functionalities 
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;   
        }

        public string GenerateToken(User user, bool isOperator)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var myClaims = new[]
            {
                new Claim("id", SymmetricEncryption.Encrypt(_config["SECRET_KEY"],Convert.ToString(user.Id))),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, isOperator? "Operator" : "User")
            };

            var token = new JwtSecurityToken(
              claims: myClaims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);


            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
