
namespace Archz.Users.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddCore()
            .AddCustomSettings(builder.Configuration)
            .AddCustomDbContext(builder.Configuration)
            .AddIdentity(builder.Configuration)
            .AddCustomHealthChecks(builder.Configuration)
            .AddCustomServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
