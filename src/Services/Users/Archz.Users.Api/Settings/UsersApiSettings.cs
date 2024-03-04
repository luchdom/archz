using System.ComponentModel.DataAnnotations;

namespace Archz.Users.Api.Settings;

public sealed class UsersApiSettings
{
    public const string Settings = "Settings";

    [Required]
    public bool LoginAfterRegister { get; set; }
}
