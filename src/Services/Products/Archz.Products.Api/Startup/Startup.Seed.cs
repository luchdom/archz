namespace Archz.Users.Api;

internal static partial class Startup
{
    public static async Task SeedData(this IHost app, IConfiguration configuration)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
        using (var scope = scopedFactory?.CreateScope())
        {
            //var service = scope!.ServiceProvider.GetService<AppDbContextSeed>();
            ////var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            ////var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            //await service.SeedAsync();
        }
    }
}
