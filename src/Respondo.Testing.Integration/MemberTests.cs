using System.Net;
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
    }
}