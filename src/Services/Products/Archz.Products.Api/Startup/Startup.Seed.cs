using Archz.Products.Api.Infra.Seed;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static async Task SeedData(this IHost app, IConfiguration configuration)
    {
        var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopedFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<AppDbContextSeed>();
            await service.SeedAsync();
        }
    }
}
