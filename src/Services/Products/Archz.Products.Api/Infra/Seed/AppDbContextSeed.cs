using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra.Seed;

public class AppDbContextSeed(
    ILogger<AppDbContextSeed> logger,
    AppWriteDbContext dbContext)
{
    public async Task<bool> SeedAsync()
    {
        logger.LogInformation("Migrating database");
        //For non-productive environments only
        await dbContext.Database.MigrateAsync();
        return true;
    }
}
