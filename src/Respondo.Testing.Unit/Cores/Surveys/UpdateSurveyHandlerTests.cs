using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Respondo.Core.Surveys;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Testing.Unit.Helpers;
using Wolverine;

namespace Respondo.Testing.Unit.Cores.Surveys;

public class UpdateSurveyHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private readonly UpdateSurveyHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        Substitute.For<ILogger<UpdateSurveyHandler>>());

    [Fact]
    public async Task Should_Update_Survey()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldUpdateSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateSurvey
        {
            Id = survey.Id,
            Title = "ShouldUpdateSurvey",
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.DbContext.Surveys
            .First(e => e.Id == request.Id)
            .Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Should_Publish_Event_When_Survey_Is_Updated()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldPublishEventWhenSurveyIsUpdated",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new UpdateSurvey
        {
            Id = survey.Id,
            Title = "ShouldPublishEventWhenSurveyIsUpdated",
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<SurveyUpdated>()
            .Id.Should().NotBeEmpty();
    }
}