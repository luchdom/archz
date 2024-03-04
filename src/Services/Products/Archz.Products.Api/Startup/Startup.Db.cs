using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Archz.Products.Api.Infra;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO - split read and write dbcontexts
        string connectionString = configuration.GetConnectionString("Database")!;

        services
            .AddDbContext<AppWriteDbContext>(options =>
            {
                options
                .UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    })
                .UseSnakeCaseNamingConvention();
                //TODO - AddInterceptors to trigger domain events
            },
            ServiceLifetime.Scoped);

        services.AddDbContext<AppReadDbContext>(
           options => options
               .UseSqlServer(connectionString)
               .UseSnakeCaseNamingConvention()
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }
}
