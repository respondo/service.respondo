using FluentAssertions;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class DeletePartyMemberTests(UnitFixture<PartiesDbContext> dbFixture) : IClassFixture<UnitFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Delete_Party_Member()
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
            Name = "Should_Delete_Party_Member",
            Email = "Should_Delete_Party_Member@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party);

        var member = new Member
        {
            Id = Guid.NewGuid(),
            Name = "Member 1",
            Party = party
        };
        
        await dbFixture.DbContext.Members.AddAsync(member);
        
        await dbFixture.DbContext.SaveChangesAsync();
        
        #endregion

        var request = new DeletePartyMember
        {
            PartyId = party.Id,
            Id = member.Id,
            ProfileId = profile.Id,
        };
        
        var @event = await new DeletePartyMemberHandler().Handle(request, dbFixture.DbContext);
        
        @event.Should().NotBeNull();
        @event?.Id.Should().Be(member.Id);
        

    }
}