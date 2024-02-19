using Archz.Users.Api.Domain.AggregateModels.RoleAggregate;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        builder.Entity<User>(entity => { entity.ToTable(name: "users"); });
        builder.Entity<Role>(entity => { entity.ToTable(name: "roles"); });
        builder.Entity<UserRole>(entity => { entity.ToTable("user_roles"); });
        builder.Entity<UserClaim>(entity => { entity.ToTable("user_claims"); });
        builder.Entity<UserLogin>(entity => { entity.ToTable("user_logins"); });
        builder.Entity<UserToken>(entity => { entity.ToTable("user_tokens"); });
        builder.Entity<RoleClaim>(entity => { entity.ToTable("role_claims"); });
    }
}
