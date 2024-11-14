using FluentAssertions;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class GetPartyTests(UnitFixture<PartiesDbContext> dbFixture) : IClassFixture<UnitFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Get_Party()
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
            Name = "Should_Get_Party",
            Email = "Should_Get_Party@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetParty
        {
            Id = party.Id,
            ProfileId = profile.Id
        };

        var response = await new GetPartyHandler().Handle(request, dbFixture.DbContext);

        response.Should().NotBeNull();
        response?.Id.Should().Be(party.Id);
        response?.Name.Should().Be(party.Name);
        response?.Email.Should().Be(party.Email);
    }

    [Fact]
    public async Task Should_Get_Party_With_Members()
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
            Name = "Should_Get_Party_With_Members",
            Email = "Should_Get_Party_With_Members@email.com",
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

        var request = new GetParty
        {
            Id = party.Id,
            ProfileId = profile.Id
        };

        var response = await new GetPartyHandler().Handle(request, dbFixture.DbContext);

        response.Should().NotBeNull();
        response?.Id.Should().Be(party.Id);
        response?.Name.Should().Be(party.Name);
        response?.Email.Should().Be(party.Email);
        response?.Members.Should().HaveCount(2);
        response?.Members.Should().Contain(m => m.Id == member1.Id && m.Name == member1.Name);
        response?.Members.Should().Contain(m => m.Id == member2.Id && m.Name == member2.Name);
    }

    [Fact]
    public async Task Should_Return_Default_If_Party_Is_Not_Found()
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

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetParty
        {
            Id = Guid.NewGuid(),
            ProfileId = profile.Id
        };

        var response = await new GetPartyHandler().Handle(request, dbFixture.DbContext);

        response.Should().BeNull();
    }
}