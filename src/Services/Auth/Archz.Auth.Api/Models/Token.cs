using OpenIddict.EntityFrameworkCore.Models;

namespace Archz.Auth.Api.Models;

public class Token : OpenIddictEntityFrameworkCoreToken<long, Application, Authorization>
{
}
