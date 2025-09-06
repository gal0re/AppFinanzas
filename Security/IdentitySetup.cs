using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AppFinanzas.Security;
using AppFinanzas.Infrastructure.Persistence;

namespace AppFinanzas.Security
{
    public static class IdentitySetup
    {
        public static IServiceCollection AddIdentityAndJwt(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<FinanzasContext>()
                .AddDefaultTokenProviders();

            var key = config["Jwt:Key"] ?? "8c2b3f7c0c874f91abf0d5a0f2e41e47d2c43f9f0e924f3ba91db47d30917c51";
            var issuer = config["Jwt:Issuer"] ?? "FinanzasApi";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            return services;
        }
    }
}
