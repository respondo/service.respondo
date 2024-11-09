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

public class DeleteQuestionHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private readonly DeleteQuestionHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        Substitute.For<ILogger<DeleteQuestionHandler>>());

    [Fact]
    public async Task Should_Delete_Question()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldDeleteQuestion",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldDeleteQuestion",
            Required = true,
            Survey = survey
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteQuestion()
        {
            QuestionId = question.Id,
            SurveyId = survey.Id,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.DbContext.Questions
            .Should().NotContain(e => e.Id == request.QuestionId);
    }

    [Fact]
    public async Task Should_Publish_Event_When_Question_Is_Deleted()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldPublishEventWhenQuestionIsDeleted",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldPublishEventWhenQuestionIsDeleted",
            Required = true,
            Survey = survey
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new DeleteQuestion()
        {
            QuestionId = question.Id,
            SurveyId = survey.Id,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<QuestionDeleted>()
            .Id.Should().NotBeEmpty();
    }
}