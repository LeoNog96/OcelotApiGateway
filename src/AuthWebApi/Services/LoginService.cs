using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthWebApi.Jwt;
using AuthWebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebApi.Services
{
    public class LoginService
    {
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginService(TokenConfigurations tokenConfigurations)
        {
            _tokenConfigurations = tokenConfigurations;
        }

        public object Auth (Login login)
        {
            if (login.UserName == null || login.Password == null)
            {
                throw new Exception ("Usuario nulo");
            }

            const bool auth = true;

            if (auth)
            {
                return TokenGenerate ();
            }
        }

        /// <summary>
        /// Gerador de Token
        /// </summary>
        /// <returns>Objeto com o token gerado</returns>
        private object TokenGenerate ()
        {
            var now = DateTime.UtcNow;

            // var claims = new Claim[]
            // {
            //     new Claim(JwtRegisteredClaimNames.Sub, name),
            //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //     new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            // };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigurations.SecretKey));

            var expires = now.AddDays(_tokenConfigurations.Days);

            var jwt = new JwtSecurityToken(
                issuer: _tokenConfigurations.Issuer,
                audience: _tokenConfigurations.Audience,
                // claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new
            {
                access_token = encodedJwt,
                expires_in = expires
            };
        }
    }
}