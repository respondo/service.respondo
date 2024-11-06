using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Common;
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
using Wolverine.Runtime;

namespace Respondo.Testing.Integration;

[Collection("Integration Tests")]
public class MemberTests(TestFactory<Program> factory) : IClassFixture<TestFactory<Program>>
{
    [Fact]
    public async Task Should_Add_Member_To_Party()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Add_Member_To_Party",
            Email = "Should_Add_Member_To_Party@email.com",
            Password = "password1234!"
        });

        var occasionCreationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "Should_Add_Member_To_Party"
        });

        var occasionCreationResponseContent = await occasionCreationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = occasionCreationResponseContent!.Id;

        var partyCreationResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party", new CreatePartyModel
        {
            Name = "Should_Add_Member_To_Party",
            Email = "Should_Add_Member_To_Party@email.com"
        });

        var partyCreationResponseContent = await partyCreationResponse.Content.ReadFromJsonAsync<CreatePartyResponse>();
        var partyId = partyCreationResponseContent!.Id;

        #endregion

        var addMemberResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party/{partyId}/member", new AddPartyMemberRequest
        {
            Name = "New Member"
        });

        addMemberResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);

        await Task.Delay(1000);
        
        var dbContext = factory.Services.GetRequiredService<PartiesDbContext>();

        var party = await dbContext.Parties
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == partyId);

        party.Should().NotBeNull();
        party!.Members.Should().Contain(m => m.Name == "New Member");
        
        
        var s = factory.Services.GetService<IWolverineRuntime>();
    }
    
    [Fact]
    public async Task Should_Delete_Member_From_Party()
    {
        #region Setup

        var client = factory.CreateClientWithoutRedirect();
        await client.PostAsJsonAsync("/api/Authentication/register", new RegisterModel
        {
            Username = "Should_Delete_Member_From_Party",
            Email = "Should_Delete_Member_From_Party@email.com",
            Password = "password1234!"
        });

        var occasionCreationResponse = await client.PostAsJsonAsync("/api/Occasion", new CreateOccasionModel
        {
            Name = "Should_Delete_Member_From_Party"
        });

        var occasionCreationResponseContent = await occasionCreationResponse.Content.ReadFromJsonAsync<CreateOccasionResponse>();
        var occasionId = occasionCreationResponseContent!.Id;

        var partyCreationResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party", new CreatePartyModel
        {
            Name = "Should_Delete_Member_From_Party",
            Email = "Should_Delete_Member_From_Party@email.com"
        });

        var partyCreationResponseContent = await partyCreationResponse.Content.ReadFromJsonAsync<CreatePartyResponse>();
        var partyId = partyCreationResponseContent!.Id;

        var addMemberResponse = await client.PostAsJsonAsync($"api/Occasion/{occasionId}/party/{partyId}/member", new AddPartyMemberRequest
        {
            Name = "New Member"
        });

        addMemberResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);

        await Task.Delay(1000);

        var partyInfo = await client.GetFromJsonAsync<GetPartyResponse>($"api/Occasion/{occasionId}/party/{partyId}");

        #endregion
        
        var response = await client.DeleteAsync($"api/Occasion/{occasionId}/party/{partyId}/member/{partyInfo!.Members[0].Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        
        await Task.Delay(1000);
        
        var dbContext = factory.Services.GetRequiredService<PartiesDbContext>();
        
        var party = await dbContext.Parties
            .Include(p => p.Members)
            .FirstAsync(p => p.Id == partyId);
        
        party.Members.Should().BeEmpty();
    }
}