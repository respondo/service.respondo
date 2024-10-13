using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Contracts.Entities;
using Respondo.Core.Occasions.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Occasions;

public class CreateOccasionTests(DbContextFixture<OccasionDbContext> dbFixture) : IClassFixture<DbContextFixture<OccasionDbContext>>
{
    [Theory]
    [InlineData("b686c655-2892-4f35-94ba-a69c7b9dda41")]
    public async Task Should_Create_Occasion(string profileId)
    {
        #region Setup

        await dbFixture.DbContext.Profiles.AddAsync(new Profile
        {
            Id = Guid.Parse(profileId)
        });

        await dbFixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new CreateOccasion
        {
            Name = "name",
            ProfileId = Guid.Parse(profileId)
        };

        var (response, @event) = await new CreateOccasionHandler().Handle(request, dbFixture.DbContext);

        response.Id.Should().NotBeEmpty();
        @event.Should().NotBeNull();

        dbFixture.DbContext.Profiles
            .Include(profile => profile.Occasions)
            .First().Occasions.Should().NotBeEmpty();
    }
}