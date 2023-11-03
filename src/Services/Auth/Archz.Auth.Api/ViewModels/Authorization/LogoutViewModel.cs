using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Archz.Auth.Api.ViewModels.Authorization;

public class LogoutViewModel
{
    [BindNever]
    public string RequestId { get; set; }
}
