using Archz.Users.Api.Domain.AggregateModels.RoleAggregate;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Archz.Users.Api.Infra.Seed;

public class AppDbContextSeed(
    ILogger<AppDbContextSeed> logger,
    AppDbContext dbContext,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
{
    public async Task<bool> SeedAsync()
    {
        logger.LogInformation("Migrating database");
        //For non-productive environments only
        await dbContext.Database.MigrateAsync();

        if (!await roleManager.Roles.AnyAsync())
        {
            logger.LogInformation("Creating default roles");
            //Seed Roles
            await roleManager.CreateAsync(Role.Admin);
            await roleManager.CreateAsync(Role.User);
        }

        var email = "admin@admin.com.br";
        if (await userManager.FindByEmailAsync(email) is null)
        {
            logger.LogInformation("Creating default admin user");
            
            //Seed Default Admin User
            var defaultUser = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var result = await userManager.CreateAsync(defaultUser, "P@ssword@135");
            if (result.Succeeded) 
            {
                logger.LogInformation("Adding default admin user to role Admin");
                await userManager.AddToRoleAsync(defaultUser, Role.Admin.Name!);
            }
            
        }
        return true;
    }
}
