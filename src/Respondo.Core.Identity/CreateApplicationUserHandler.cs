using Microsoft.AspNetCore.Identity;
using Respondo.Core.Identity.Contracts;
using Respondo.Core.Identity.Contracts.Entities;

namespace Respondo.Core.Identity;

public sealed record CreateApplicationUserHandler
{
    public async Task<(IdentityResult?, ApplicationUserCreated?)> Handle(CreateApplicationUser request, UserManager<ApplicationUser> userManager)
    {
        var userId = Guid.CreateVersion7(TimeProvider.System.GetUtcNow());
        
        var user = new ApplicationUser
        {
            Id = userId.ToString(),
            UserName = request.Username,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        return result.Succeeded switch
        {
            true => (result, new ApplicationUserCreated(userId)),
            _ => (result, default)
        };
    }
}