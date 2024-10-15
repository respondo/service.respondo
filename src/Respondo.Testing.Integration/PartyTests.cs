using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Api;
using Respondo.Api.Models.Authentication;
using Respondo.Api.Models.Occasion;
using Respondo.Api.Models.Party;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Integration.Helpers;

namespace Respondo.Testing.Integration;

public class PartyTests(TestFactory<Program> factory) : IClassFixture<TestFactory<Program>>
{
    [Fact]
    public async Task Should_Get_Party()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Get_Party",
            Email = "Should_Get_Party@email.com",
            Password = "password1234!"
        });

        var occasionCreationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "Should_Get_Party"
        });

        var occasionCreationResponseContent = await occasionCreationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = occasionCreationResponseContent!.Id;

        var partyCreationResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party", new CreatePartyModel
        {
            Name = "Should_Get_Party",
            Email = "Should_Get_Party@email.com"
        });

        var partyCreationResponseContent = await partyCreationResponse.Content.ReadFromJsonAsync<CreatePartyResponse>();
        var partyId = partyCreationResponseContent!.Id;

        #endregion

        var response = await client.GetAsync($"/api/Occasion/{occasionId}/party/{partyId}");

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadFromJsonAsync<GetPartyResponse>();

        content!.Id.Should().Be(partyId);
        content.Name.Should().Be("Should_Get_Party");
        content.Email.Should().Be("Should_Get_Party@email.com");
    }

    [Fact]
    public async Task Should_Create_Party()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Create_Party",
            Email = "Should_Create_Party@email.com",
            Password = "password1234!"
        });

        var occasionCreationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "Should_Create_Party"
        });

        var occasionCreationResponseContent = await occasionCreationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = occasionCreationResponseContent!.Id;

        #endregion

        var response = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party", new CreatePartyModel
        {
            Name = "Should_Create_Party",
            Email = "Should_Create_Party@email.com"
        });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadFromJsonAsync<CreatePartyResponse>();
        content.Should().NotBeNull();
        content!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_Delete_Party()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Delete_Party",
            Email = "Should_Delete_Party@email.com",
            Password = "password1234!"
        });

        var occasionCreationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "Should_Delete_Party"
        });

        var occasionCreationResponseContent = await occasionCreationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = occasionCreationResponseContent!.Id;

        var partyCreationResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party", new CreatePartyModel
        {
            Name = "Should_Delete_Party",
            Email = "Should_Delete_Party@email.com"
        });

        var partyCreationResponseContent = await partyCreationResponse.Content.ReadFromJsonAsync<CreatePartyResponse>();
        var partyId = partyCreationResponseContent!.Id;

        #endregion

        var response = await client.DeleteAsync($"api/Occasion/{occasionId}/party/{partyId}");

        response.IsSuccessStatusCode.Should().BeTrue();

        await Task.Delay(1000);

        var dbContext = factory.Services.GetRequiredService<PartiesDbContext>();

        var party = await dbContext.Parties
            .FirstOrDefaultAsync(p => p.Id == partyId);

        party.Should().BeNull();
    }
}