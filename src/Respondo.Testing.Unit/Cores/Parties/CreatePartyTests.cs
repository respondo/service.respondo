using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class CreatePartyTests(DbContextFixture<PartiesDbContext> dbFixture) : IClassFixture<DbContextFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Create_Party()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await dbFixture.DbContext.Profiles.AddAsync(profile);

        var occasionId = Guid.NewGuid();

        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = occasionId,
            Profile = profile
        });

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new CreateParty
        {
            OccasionId = occasionId,
            Name = "ShouldCreateParty",
            Email = "ShouldCreateParty@email.com",
            ProfileId = profile.Id
        };

        var (response, @event) = await new CreatePartyHandler().Handle(request, dbFixture.DbContext);

        response.Id.Should().NotBeEmpty();
        @event.Id.Should().NotBeEmpty();
        
        var party = dbFixture.DbContext.Parties
            .Include(party => party.Occasion)
            .First();

        party.Id.Should().Be(response.Id);
        party.Name.Should().Be(request.Name);
        party.Email.Should().Be(request.Email);
        party.Occasion.Id.Should().Be(occasionId);
    }
}