using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Respondo.Api;
using Respondo.Api.Models.Authentication;
using Respondo.Api.Models.Occasion;
using Respondo.Core.Occasions.Contracts;
using Respondo.Testing.Integration.Helpers;

namespace Respondo.Testing.Integration;

public class OccasionTests(TestFactory<Program> factory) : IClassFixture<TestFactory<Program>>
{
    [Fact]
    public async Task Should_Get_Occasion()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Get_Occasion",
            Email = "Should_Get_Occasion@email.com",
            Password = "password1234!"
        });
        
        var creationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "ShouldGetOccasion"
        });

        var creationContent = await creationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = creationContent!.Id;
        
        #endregion
        
        var response = await client.GetAsync($"/api/Occasion/{occasionId}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var content = await response.Content.ReadFromJsonAsync<GetOccasionResponse>();
        
        content!.Id.Should().Be(occasionId);
        content.Name.Should().Be("ShouldGetOccasion");
    }

    [Fact]
    public async Task Should_Create_Occasion()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Create_Occasion",
            Email = "Should_Create_Occasion@email.com",
            Password = "password1234!"
        });

        #endregion
        
        var response = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "ShouldCreateOccasion"
        });
        
        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadFromJsonAsync<CreateOccasionResponse>();

        content!.Id.Should().NotBeEmpty();
    }
}