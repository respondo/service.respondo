using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Respondo.Core.Surveys;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Core.Surveys.Services;
using Respondo.Testing.Unit.Helpers;
using Wolverine;

namespace Respondo.Testing.Unit.Cores.Surveys;

public class AnswerSurveyHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private static readonly IAnswerValidationService ValidationService = Substitute.For<IAnswerValidationService>();
    
    private readonly AnswerSurveyHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        ValidationService
        , Substitute.For<ILogger<AnswerSurveyHandler>>());

    [Fact]
    public async Task Should_Answer_Survey()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldAnswerSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldAnswerSurvey",
            Required = true,
            Survey = survey
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.SaveChangesAsync();

        ValidationService
            .IsValid(Arg.Is<Question>(arg => arg.Id == question.Id), Arg.Any<string?>())
            .Returns(true);
        
        #endregion
        
        var request = new AnswerSurvey
        {
            SurveyId = survey.Id,
            PartyId = Guid.CreateVersion7(),
            AnswersByMember = [(Guid.CreateVersion7(), new (Guid, string?)[] { (question.Id, "true") })]
        };
        
        await _handler.Handle(request, CancellationToken.None);

        var answer = await fixture.DbContext.Answers
            .FirstAsync(e => e.Question.Id == question.Id);

        answer.Value.Should().Be("true");
        answer.MemberId.Should().Be(request.AnswersByMember[0].MemberId);
    }
    
    [Fact]
    public async Task Should_Publish_Event_When_Survey_Is_Answered()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldPublishEventWhenSurveyIsAnswered",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldPublishEventWhenSurveyIsAnswered",
            Required = true,
            Survey = survey
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.SaveChangesAsync();

        #endregion
        
        var request = new AnswerSurvey
        {
            SurveyId = survey.Id,
            PartyId = Guid.CreateVersion7(),
            AnswersByMember = [(Guid.CreateVersion7(), new (Guid, string?)[1] { (question.Id, "true") })]
        };
        
        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<SurveyAnswered>()
            .Id.Should().NotBeEmpty();
    }
}