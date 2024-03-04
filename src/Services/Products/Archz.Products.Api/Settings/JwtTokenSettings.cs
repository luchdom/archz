using System.ComponentModel.DataAnnotations;

namespace Archz.Products.Api.Settings;

public sealed class JwtTokenSettings
{
    public const string Settings = "JwtTokenSettings";

    [Required]
    public string ValidIssuer { get; set; }
    [Required]
    public string ValidAudience { get; set; }
    [Required]
    public string SymmetricSecurityKey { get; set; }
    [Required]
    public int ExpirationMinutes { get; set; }
}
