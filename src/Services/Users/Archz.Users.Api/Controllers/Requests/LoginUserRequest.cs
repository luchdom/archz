namespace Archz.Users.Api.Controllers.Requests;

public sealed record LoginUserRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }

    public override string ToString() => nameof(LoginUserRequest);
}
