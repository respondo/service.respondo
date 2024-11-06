using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class AddPartyMemberTests(DbContextFixture<PartiesDbContext> dbFixture) : IClassFixture<DbContextFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Add_Member_To_Party()
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
            Name = "Should_Add_Member_To_Party",
            Email = "Should_Add_Member_To_Party@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new AddPartyMember
        {
            PartyId = party.Id,
            ProfileId = profile.Id,
            Name = "Should_Add_Member_To_Party"
        };

        var @event = await new AddPartyMemberHandler().Handle(request, dbFixture.DbContext);

        @event!.Id.Should().NotBeEmpty();

        var member = dbFixture.DbContext.Members
            .Include(member => member.Party)
            .First();

        member.Id.Should().NotBeEmpty();
        member.Name.Should().Be(request.Name);
        member.Party.Id.Should().Be(party.Id);
    }
}