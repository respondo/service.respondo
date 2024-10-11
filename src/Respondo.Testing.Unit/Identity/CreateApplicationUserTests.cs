using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Respondo.Core.Identity;
using Respondo.Core.Identity.Contracts;
using Respondo.Core.Identity.Contracts.Entities;
using Xunit;

namespace Respondo.Testing.Unit.Identity;

public class CreateApplicationUserTests
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateApplicationUserTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task Should_Raise_ApplicationUserCreated_Event_When_User_Is_Created()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(Task.FromResult(IdentityResult.Success));
        
        var request = new CreateApplicationUser
        {
            Username = "username",
            Email = "username@email.com",
            Password = "password"
        };

        var (result, @event) = await new CreateApplicationUserHandler().Handle(request, _userManager);

        result?.Succeeded.Should().BeTrue();
        @event.Should().NotBeNull();
        @event!.ApplicationUserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_Not_Raise_ApplicationUserCreated_Event_When_User_Is_Not_Created()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));

        var request = new CreateApplicationUser
        {
            Username = "username",
            Email = "username@email.com",
            Password = "password"
        };
        
        var (result, @event) = await new CreateApplicationUserHandler().Handle(request, _userManager);

        result?.Succeeded.Should().BeFalse();
        @event.Should().BeNull();
    }
}