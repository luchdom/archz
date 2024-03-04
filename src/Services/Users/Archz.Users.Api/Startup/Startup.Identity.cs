using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using Archz.Users.Api.Infra;
using Archz.Users.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Archz.Users.Api.Domain.AggregateModels.RoleAggregate;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityProvider()
            .AddAuth(configuration);
        return services;
    }
    private static IServiceCollection AddIdentityProvider(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AppDbContext>();
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
