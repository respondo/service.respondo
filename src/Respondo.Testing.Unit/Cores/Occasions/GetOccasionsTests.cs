using FluentAssertions;
using Respondo.Core.Occasions;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Entities;
using Respondo.Core.Occasions.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Occasions;

public class GetOccasionsTests(DbContextFixture<OccasionDbContext> dbFixture) : IClassFixture<DbContextFixture<OccasionDbContext>>
{
    [Fact]
    public async Task Should_Get_Occasions()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await dbFixture.DbContext.Profiles.AddAsync(profile);

        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = Guid.NewGuid(),
            Name = "ShouldGetOccasions1",
            Profile = profile
        });
        
        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = Guid.NewGuid(),
            Name = "ShouldGetOccasions2",
            Profile = profile
        });

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetOccasions
        {
            ProfileId = profile.Id
        };
        
        var result = await new GetOccasionsHandler().Handle(request, dbFixture.DbContext);

        result.Occasions.Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_Not_Get_Occasions_When_Profile_Is_Different()
    {
        #region Setup
     
        var profile1 = new Profile
        {
            Id = Guid.NewGuid()
        };
        
        var profile2 = new Profile
        {
            Id = Guid.NewGuid()
        };

        await dbFixture.DbContext.Profiles.AddAsync(profile1);
        await dbFixture.DbContext.Profiles.AddAsync(profile2);

        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = Guid.NewGuid(),
            Name = "ShouldGetOccasions1",
            Profile = profile1
        });
        
        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = Guid.NewGuid(),
            Name = "ShouldGetOccasions2",
            Profile = profile1
        });
        
        await dbFixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = Guid.NewGuid(),
            Name = "ShouldGetOccasions3",
            Profile = profile2
        });


        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new GetOccasions
        {
            ProfileId = profile1.Id
        };
        
        var result = await new GetOccasionsHandler().Handle(request, dbFixture.DbContext);
        
        result.Occasions.Should().HaveCount(2);
    }
}