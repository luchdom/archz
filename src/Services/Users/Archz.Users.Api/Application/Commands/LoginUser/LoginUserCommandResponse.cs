using System.Text.Json.Serialization;

namespace Archz.Users.Api.Application.Commands.LoginUser;

public class LoginUserCommandResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
