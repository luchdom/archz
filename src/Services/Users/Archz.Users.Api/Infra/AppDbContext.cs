using Archz.Users.Api.Domain.AggregateModels.RoleAggregate;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Archz.Users.Api.Infra;

public class AppDbContext : IdentityDbContext
    <User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
