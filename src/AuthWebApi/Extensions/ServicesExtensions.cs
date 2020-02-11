using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AuthWebApi.Jwt;
using AuthWebApi.Services;

namespace AuthWebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<LoginService>();
        }
        public static void ConfigureTokenConfigurations (this IServiceCollection services, IConfiguration config)
        {
            var signingConfigurations = new SigningConfigurations ();

            services.AddSingleton (signingConfigurations);

            var tokenConfigurations = new TokenConfigurations ();

            new ConfigureFromConfigurationOptions<TokenConfigurations> (
                    config.GetSection ("TokenConfigurations"))
                .Configure (tokenConfigurations);

            services.AddSingleton (tokenConfigurations);
        }
    }
}