using Archz.Application.Core.Startup;
using Archz.Users.Api;
namespace Archz.Products.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddCore<Program>()
            .AddCustomSettings(builder.Configuration)
            .AddCustomDbContext(builder.Configuration)
            .AddIdentity(builder.Configuration)
            .AddCustomHealthChecks(builder.Configuration)
            .AddCustomServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.SeedData(builder.Configuration);
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
