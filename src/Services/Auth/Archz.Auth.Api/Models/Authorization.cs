using OpenIddict.EntityFrameworkCore.Models;

namespace Archz.Auth.Api.Models;

public class Authorization : OpenIddictEntityFrameworkCoreAuthorization<long, Application, Token>
{
}
