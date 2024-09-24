using Microsoft.AspNetCore.Identity;
using Respondo.Core.Entities.Identity;

namespace Respondo.Core.Identity;

public sealed record CreateUserHandler
{
    public async Task<(IdentityResult, ApplicationUserCreated)> Handle(CreateUser command, UserManager<ApplicationUser> userManager)
    {
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId.ToString(),
            UserName = command.Username,
            Email = command.Email
        };

        var result = await userManager.CreateAsync(user, command.Password);

        var @event = new ApplicationUserCreated(userId);

        return (result, @event);
    }
}