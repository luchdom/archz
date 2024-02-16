using Microsoft.AspNetCore.Identity;

namespace Archz.Users.Api.Domain.AggregateModels.RoleAggregate;

public class Role : IdentityRole<int>
{
    public string Description { get; set; }
}
