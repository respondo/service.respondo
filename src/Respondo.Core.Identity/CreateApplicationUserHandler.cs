using Microsoft.AspNetCore.Identity;
using Respondo.Core.Identity.Contracts;
using Respondo.Core.Identity.Contracts.Entities;

namespace Respondo.Core.Identity;

public sealed record CreateApplicationUserHandler
{
    public async Task<ApplicationUserCreated?> Handle(CreateApplicationUser command, UserManager<ApplicationUser> userManager)
    {
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId.ToString(),
            UserName = command.Username,
            Email = command.Email
        };

        var result = await userManager.CreateAsync(user, command.Password);

        return result.Succeeded ? new ApplicationUserCreated(userId) : default;
    }
}