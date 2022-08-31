using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    //JSON Web Token structure
    /*
    Header: 
    {
        "alg": "HS256",
        "typ": "JWT"
    }

    Payload:
    {
        "sub": "1234567890",
        "name": "John Doe",
        "admin": true
    }

    Signature:
    HMACSHA256
    (
        base64UrlEncode(header) + "." +
        base64UrlEncode(payload),
        secret
    )
    **/
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            //Create claims (user information). 
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
                //the rest of user information... (ex: user role)
            };

            SigningCredentials creds = new(_key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //Create token
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}