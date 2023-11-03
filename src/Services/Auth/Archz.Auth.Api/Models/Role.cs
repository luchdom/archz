using Microsoft.AspNetCore.Identity;

namespace Archz.Auth.Api.Models;

public class Role : IdentityRole
{
    public string Description { get; set; }
}
