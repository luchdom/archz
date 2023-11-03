using OpenIddict.EntityFrameworkCore.Models;

namespace Archz.Auth.Api.Models;

public class Application : OpenIddictEntityFrameworkCoreApplication<long, Authorization, Token>
{
}
