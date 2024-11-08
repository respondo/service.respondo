using FluentAssertions;
using Respondo.Core.Occasions;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Entities;
using Respondo.Core.Occasions.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Occasions;

public class GetOccasionTests(UnitFixture<OccasionDbContext> dbFixture) : IClassFixture<UnitFixture<OccasionDbContext>>
{
    [Fact]
    public async Task Should_Get_Occasion()
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
            Name = "ShouldGetOccasion",
            Profile = profile
        });

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetOccasion
        {
            Id = occasionId,
            ProfileId = profile.Id
        };
        
        var result = await new GetOccasionHandler().Handle(request, dbFixture.DbContext);

        result.Should().NotBeNull();
        result!.Id.Should().Be(occasionId);
        result.Name.Should().Be("ShouldGetOccasion");
    }

    [Fact]
    public async Task Should_Not_Get_Occasion_When_Profile_Is_Different()
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
            Name = "ShouldGetOccasion",
            Profile = profile
        });

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetOccasion
        {
            Id = occasionId,
            ProfileId = Guid.NewGuid()
        };
        
        var result = await new GetOccasionHandler().Handle(request, dbFixture.DbContext);

        result.Should().BeNull();
    }
}