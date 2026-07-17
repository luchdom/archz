namespace Archz.Products.Tests.Fixtures;

internal static class ProductTestSettings
{
    public static IReadOnlyDictionary<string, string?> Configuration { get; } =
        new Dictionary<string, string?>
        {
            ["ConnectionStrings:Database"] = "Server=(local);Database=archz-products-tests;Trusted_Connection=True;TrustServerCertificate=True",
            ["JwtTokenSettings:ValidIssuer"] = "Archz.Tests",
            ["JwtTokenSettings:ValidAudience"] = "Archz.Tests",
            ["JwtTokenSettings:SymmetricSecurityKey"] = "test-signing-key-that-is-long-enough-for-hmac-sha256",
            ["JwtTokenSettings:ExpirationMinutes"] = "30"
        };

    public static void ApplyToEnvironment()
    {
        foreach (var setting in Configuration)
        {
            Environment.SetEnvironmentVariable(setting.Key.Replace(":", "__"), setting.Value);
        }
    }
}
