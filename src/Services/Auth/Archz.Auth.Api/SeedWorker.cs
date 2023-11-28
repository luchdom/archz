using Archz.Auth.Api.Data;
using Archz.Auth.Api.Models;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Archz.Auth.Api;

public class SeedWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedWorker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        await CreateClients(manager, cancellationToken);
    }

    private static async Task CreateClients(IOpenIddictApplicationManager manager, CancellationToken cancellationToken)
    {
        var clients = new List<Client>()
        {
            new ("authz-web-client", "AuthZ web-client application","https://localhost:7281/"),
            new ("authz-oidc-debugger", "AuthZ oidc-debugger","https://localhost:7281/authentication/logout-callback", "https://oidcdebugger.com/debug" ),
            new ("authz-postman-local-client", "AuthZ postman","https://localhost:7281/authentication/logout-callback", "https://oauth.pstmn.io/v1/callback")
        };

        foreach (var client in clients)
        {
            await CreateClient(manager, client, cancellationToken);
        }
    }

    private static async Task CreateClient(IOpenIddictApplicationManager manager, Client client, CancellationToken cancellationToken)
    {
        if (await manager.FindByClientIdAsync(client.Id, cancellationToken) == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = client.Id,
                ConsentType = ConsentTypes.Explicit,
                DisplayName = client.DisplayName,
                Type = ClientTypes.Public,
                PostLogoutRedirectUris =
                {
                    new Uri(client.PostLogoutRedirectUris)
                },
                RedirectUris =
                {
                    new Uri(client.RedirectUris)
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                }
                //Requirements =
                //{
                //    Requirements.Features.ProofKeyForCodeExchange
                //}
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
