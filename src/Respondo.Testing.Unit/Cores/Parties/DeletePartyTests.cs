using FluentAssertions;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class DeletePartyTests(DbContextFixture<PartiesDbContext> dbFixture) : IClassFixture<DbContextFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Delete_Party()
    {
        #region Setup

        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await dbFixture.DbContext.Profiles.AddAsync(profile);

        var occasion = new Occasion
        {
            Id = Guid.NewGuid(),
            Profile = profile
        };

        await dbFixture.DbContext.Occasions.AddAsync(occasion);

        var party = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Delete_Party",
            Email = "Should_Delete_Party@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteParty
        {
            Id = party.Id,
            ProfileId = profile.Id
        };

        var @event = await new DeletePartyHandler().Handle(request, dbFixture.DbContext);

        @event.Should().NotBeNull();
        @event?.PartyId.Should().Be(party.Id);
    }

    [Fact]
    public async Task Should_Delete_Party_And_Its_Members()
    {
        #region Setup

        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await dbFixture.DbContext.Profiles.AddAsync(profile);

        var occasion = new Occasion
        {
            Id = Guid.NewGuid(),
            Profile = profile
        };

        await dbFixture.DbContext.Occasions.AddAsync(occasion);

        var party = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Delete_Party_And_Its_Members",
            Email = "Should_Delete_Party_And_Its_Members@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party);

        var member1 = new Member
        {
            Id = Guid.NewGuid(),
            Name = "Member 1",
            Party = party
        };

        var member2 = new Member
        {
            Id = Guid.NewGuid(),
            Name = "Member 2",
            Party = party
        };

        await dbFixture.DbContext.Members.AddAsync(member1);
        await dbFixture.DbContext.Members.AddAsync(member2);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteParty
        {
            Id = party.Id,
            ProfileId = profile.Id
        };

        var @event = await new DeletePartyHandler().Handle(request, dbFixture.DbContext);

        @event.Should().NotBeNull();
        @event?.PartyId.Should().Be(party.Id);

        var members = dbFixture.DbContext.Members.Where(m => m.Party.Id == party.Id).ToList();
        
        members.Should().BeEmpty();
    }
}