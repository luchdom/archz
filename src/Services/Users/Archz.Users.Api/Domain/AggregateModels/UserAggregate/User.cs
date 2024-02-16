using Microsoft.AspNetCore.Identity;

namespace Archz.Users.Api.Domain.AggregateModels.UserAggregate;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser<int> { }