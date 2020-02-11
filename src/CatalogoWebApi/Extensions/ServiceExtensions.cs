using System;
using System.Text;
using CatalogoWebApi.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CatalogoWebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureAuthentication (this IServiceCollection services, IConfiguration config)
        {
            var signingConfigurations = new SigningConfigurations ();

            services.AddSingleton (signingConfigurations);

            var tokenConfigurations = new TokenConfigurations ();

            new ConfigureFromConfigurationOptions<TokenConfigurations> (
                    config.GetSection ("TokenConfigurations"))
                .Configure (tokenConfigurations);

            services.AddSingleton (tokenConfigurations);

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.SecretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = tokenConfigurations.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenConfigurations.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o => o.DefaultAuthenticateScheme = "Bearer")
            .AddJwtBearer("Bearer", x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.TokenValidationParameters = tokenValidationParameters;
             });
        }
    }
}