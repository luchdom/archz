using Archz.Products.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuth(configuration);
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtTokenSettings = services.BuildServiceProvider().GetService<IOptions<JwtTokenSettings>>()?.Value;
        services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtTokenSettings!.ValidIssuer,
                    ValidAudience = jwtTokenSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtTokenSettings.SymmetricSecurityKey)
                    ),
                };
            });
        return services;
    }
}
