using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Archz.Architecture.Tests;

internal sealed class MvcArchitectureFactory<TProgram>(string contentRoot) : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseContentRoot(contentRoot);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configuration =>
        {
            // Users and Products read JWT settings while registering their services.
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = "Server=unused;Database=archz-architecture;Trusted_Connection=True",
                ["JwtTokenSettings:ValidIssuer"] = "Archz.Tests",
                ["JwtTokenSettings:ValidAudience"] = "Archz.Tests",
                ["JwtTokenSettings:SymmetricSecurityKey"] = "test-only-key-for-architecture-discovery",
                ["JwtTokenSettings:ExpirationMinutes"] = "30"
            });
        });
        return base.CreateHost(builder);
    }
}
