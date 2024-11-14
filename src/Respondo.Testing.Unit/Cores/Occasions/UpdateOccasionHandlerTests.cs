using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Respondo.Core.Occasions;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Entities;
using Respondo.Core.Occasions.Persistence;
using Respondo.Testing.Unit.Helpers;
using Wolverine;

namespace Respondo.Testing.Unit.Cores.Occasions;

public class UpdateOccasionHandlerTests(UnitFixture<OccasionDbContext> fixture) : IClassFixture<UnitFixture<OccasionDbContext>>
{
    private readonly UpdateOccasionHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        Substitute.For<ILogger<UpdateOccasionHandler>>());
    
    [Fact]
    public async Task Should_Update_Occasion_Name()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await fixture.DbContext.Profiles.AddAsync(profile);

        var occasionId = Guid.NewGuid();

        await fixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = occasionId,
            Name = "Should_Update_Occasion_Name",
            Profile = profile
        });

        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateOccasion
        {
            OccasionId = occasionId,
            ProfileId = profile.Id,
            Name = "NewName"
        };

        await _handler.Handle(request, CancellationToken.None);

        var updatedOccasion = fixture.DbContext.Occasions.First(occasion => occasion.Id == occasionId);

        updatedOccasion.Name.Should().Be(request.Name);
    }
    
    [Fact]
    public async Task Should_Update_Occasion_BaseUrl()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await fixture.DbContext.Profiles.AddAsync(profile);

        var occasionId = Guid.NewGuid();

        await fixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = occasionId,
            Name = "Should_Update_Occasion_BaseUrl",
            Profile = profile
        });

        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateOccasion
        {
            OccasionId = occasionId,
            ProfileId = profile.Id,
            BaseUrl = "base.url"
        };

        await _handler.Handle(request, CancellationToken.None);

        var updatedOccasion = fixture.DbContext.Occasions.First(occasion => occasion.Id == occasionId);

        updatedOccasion.BaseUrl.Should().Be(request.BaseUrl);
    }
    
    [Fact]
    public async Task Should_Update_Occasion_Logo()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await fixture.DbContext.Profiles.AddAsync(profile);

        var occasionId = Guid.NewGuid();

        await fixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = occasionId,
            Name = "Should_Update_Occasion_Logo",
            Profile = profile
        });

        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateOccasion
        {
            OccasionId = occasionId,
            ProfileId = profile.Id,
            Logo = "base64/logo"
        };

        await _handler.Handle(request, CancellationToken.None);

        var updatedOccasion = fixture.DbContext.Occasions.First(occasion => occasion.Id == occasionId);

        updatedOccasion.Logo.Should().Be(request.Logo);
    }
    
    [Fact]
    public async Task Should_Publish_Event_When_Occasion_Was_Updated()
    {
        #region Setup
     
        var profile = new Profile
        {
            Id = Guid.NewGuid()
        };

        await fixture.DbContext.Profiles.AddAsync(profile);

        var occasionId = Guid.NewGuid();

        await fixture.DbContext.Occasions.AddAsync(new Occasion
        {
            Id = occasionId,
            Name = "Should_Publish_Event_When_Occasion_Was_Updated",
            Profile = profile
        });

        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateOccasion
        {
            OccasionId = occasionId,
            ProfileId = profile.Id,
            Name = "NewNameWithEvent"
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<OccasionUpdated>();
    }
}