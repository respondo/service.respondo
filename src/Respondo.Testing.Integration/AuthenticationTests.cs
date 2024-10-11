using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Api;
using Respondo.Api.Models;
using Respondo.Core.Identity.Contracts.Entities;
using Respondo.Testing.Integration.Helpers;

namespace Respondo.Testing.Integration;

public class AuthenticationTests(TestFactory<Program> factory) : IClassFixture<TestFactory<Program>>
{
    [Fact]
    public async Task Should_Register_A_New_User()
    {
        var client = factory.CreateClientWithoutRedirect();
        var response = await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "shouldRegister",
            Email = "shouldRegister@email.com",
            Password = "shouldRegister1234!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Should().ContainKey("Set-Cookie");
    }

    [Fact]
    public async Task Should_Login_An_Existing_User()
    {
        #region Setup

        var manager = factory.Services.GetService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser
        {
            Id = Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString(),
            UserName = "shouldLogin",
            Email = "shouldLogin@email.com"
        };

        await manager?.CreateAsync(user, "shouldLogin1234!")!;

        #endregion
        
        var client = factory.CreateClientWithoutRedirect();
        
        var response = await client.PostAsJsonAsync("/api/Authentication/login", new LoginModel
        {
            Email = "shouldLogin@email.com",
            Password = "shouldLogin1234!"
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Should().ContainKey("Set-Cookie");

        response = await client.GetAsync("api/Authentication/authenticate");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}