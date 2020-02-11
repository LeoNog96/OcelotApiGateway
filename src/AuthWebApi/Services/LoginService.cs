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
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly IConfiguration _config;

        public LoginService(SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations,
            IConfiguration config)
        {
            _signingConfigurations = signingConfigurations;

            _tokenConfigurations = tokenConfigurations;

            _config = config;
        }

        public object Auth (Login login)
        {
            if (login.UserName == null || login.Password == null)
            {
                throw new Exception ("Usuario nulo");
            }

            var auth = true;

            if (auth)
            {
                return TokenGenerate ();
            }

            return null;
        }

        /// <summary>
        /// Gerador de Token
        /// </summary>
        /// <param name="user">Usuario</param>
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

            // var tokenValidationParameters = new TokenValidationParameters
            // {
            //     ValidateIssuerSigningKey = true,
            //     IssuerSigningKey = signingKey,
            //     ValidateIssuer = true,
            //     ValidIssuer = _tokenConfigurations.Issuer,
            //     ValidateAudience = true,
            //     ValidAudience =_tokenConfigurations.Audience,
            //     ValidateLifetime = true,
            //     ClockSkew = TimeSpan.Zero,
            //     RequireExpirationTime = true,
            // };

            var jwt = new JwtSecurityToken(
                issuer: _tokenConfigurations.Issuer,
                audience: _tokenConfigurations.Audience,
                // claims: claims,
                notBefore: now,
                expires: now.AddDays(_tokenConfigurations.Days),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
            };
        }
    }
}