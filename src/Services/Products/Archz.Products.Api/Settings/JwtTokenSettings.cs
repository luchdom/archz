using System.ComponentModel.DataAnnotations;

namespace Archz.Products.Api.Settings;

public sealed class JwtTokenSettings
{
    public const string Settings = "JwtTokenSettings";

    [Required]
    public required string ValidIssuer { get; set; }
    [Required]
    public required string ValidAudience { get; set; }
    [Required]
    public required string SymmetricSecurityKey { get; set; }
    [Required]
    public int ExpirationMinutes { get; set; }
}
