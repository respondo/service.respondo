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

public class DeleteSurveyHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private readonly DeleteSurveyHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        Substitute.For<ILogger<DeleteSurveyHandler>>());

    [Fact]
    public async Task Should_Delete_Survey()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldDeleteSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteSurvey
        {
            Id = survey.Id,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.DbContext.Surveys
            .FirstOrDefault(e => e.Id == request.Id)
            .Should().BeNull();
    }

    [Fact]
    public async Task Should_Publish_Event_When_Survey_Is_Deleted()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldPublishEventWhenSurveyIsDeleted",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteSurvey
        {
            Id = survey.Id,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<SurveyDeleted>()
            .Id.Should().NotBeEmpty();
    }
}