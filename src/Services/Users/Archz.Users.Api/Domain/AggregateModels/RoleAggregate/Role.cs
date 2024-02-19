using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Archz.Users.Api.Domain.AggregateModels.RoleAggregate;

public class Role : IdentityRole<int>
{
    public string Description { get; set; }

    public static Role Admin = new Role("Admin", "Administrative role - full access");
    public static Role User = new Role("User", "Regular user");

    private Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
