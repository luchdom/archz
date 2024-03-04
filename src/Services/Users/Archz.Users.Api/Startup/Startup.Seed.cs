using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Archz.Users.Api.Infra;
using Archz.Users.Api.Settings;
using Microsoft.AspNetCore.Identity;
using Archz.Users.Api.Infra.Seed;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using Archz.Users.Api.Domain.AggregateModels.RoleAggregate;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static async Task SeedData(this IHost app, IConfiguration configuration)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
        using (var scope = scopedFactory?.CreateScope())
        {
            var service = scope!.ServiceProvider.GetService<AppDbContextSeed>();
            await service!.SeedAsync();
        }
    }
}
