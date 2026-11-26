namespace Archz.Users.Api.Controllers.Requests;

public sealed record RegisterUserRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? PasswordConfirmation { get; init; }

    public override string ToString() => nameof(RegisterUserRequest);
}
