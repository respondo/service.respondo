using FluentAssertions;
using Respondo.Core.Parties;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Parties;

public class GetPartiesTests(DbContextFixture<PartiesDbContext> dbFixture) : IClassFixture<DbContextFixture<PartiesDbContext>>
{
    [Fact]
    public async Task Should_Get_Parties()
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

        var party1 = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Get_Parties1",
            Email = "Should_Get_Parties1@email.com",
            Occasion = occasion
        };
        var party2 = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Get_Parties2",
            Email = "Should_Get_Parties2@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party1);
        await dbFixture.DbContext.Parties.AddAsync(party2);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetParties
        {
            OccasionId = occasion.Id,
            ProfileId = profile.Id
        };

        var response = await new GetPartiesHandler().Handle(request, dbFixture.DbContext);

        response.Should().NotBeNull();
        response.Parties.Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_Get_Parties_With_Members()
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

        var party1 = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Get_Parties1",
            Email = "Should_Get_Parties1@email.com",
            Occasion = occasion
        };
        var party2 = new Party
        {
            Id = Guid.NewGuid(),
            Name = "Should_Get_Parties2",
            Email = "Should_Get_Parties2@email.com",
            Occasion = occasion
        };

        await dbFixture.DbContext.Parties.AddAsync(party1);
        await dbFixture.DbContext.Parties.AddAsync(party2);

        var member1 = new Member
        {
            Id = Guid.NewGuid(),
            Name = "Member 1",
            Party = party1
        };

        var member2 = new Member
        {
            Id = Guid.NewGuid(),
            Name = "Member 2",
            Party = party2
        };

        await dbFixture.DbContext.Members.AddAsync(member1);
        await dbFixture.DbContext.Members.AddAsync(member2);

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetParties
        {
            OccasionId = occasion.Id,
            ProfileId = profile.Id
        };

        var response = await new GetPartiesHandler().Handle(request, dbFixture.DbContext);

        response.Parties.Should().HaveCount(2);
        
        response.Parties[0].Members.Should().HaveCount(1);
        response.Parties[1].Members.Should().HaveCount(1);
    }
}