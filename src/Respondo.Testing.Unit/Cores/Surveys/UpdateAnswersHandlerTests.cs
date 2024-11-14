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

public class UpdateAnswersHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private static readonly IAnswerValidationService ValidationService = Substitute.For<IAnswerValidationService>();
    
    private readonly UpdateAnswersHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        ValidationService, Substitute.For<ILogger<UpdateAnswersHandler>>());

    [Fact]
    public async Task Should_Update_Answers()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldUpdateAnswers",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldUpdateAnswers",
            Required = true,
            Survey = survey
        };

        var previousAnswer = new Answer
        {
            Question = question,
            MemberId = Guid.CreateVersion7(),
            Value = "true"
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.Answers.AddAsync(previousAnswer);
        await fixture.DbContext.SaveChangesAsync();

        ValidationService
            .IsValid(Arg.Is<Question>(arg => arg.Id == question.Id), Arg.Any<string?>())
            .Returns(true);
        
        #endregion
        
        var request = new UpdateAnswers
        {
            SurveyId = survey.Id,
            PartyId = Guid.CreateVersion7(),
            AnswersByMember = [(previousAnswer.MemberId, new (Guid, string?)[] { (question.Id, "false") })]
        };
        
        await _handler.Handle(request, CancellationToken.None);

        var answer = await fixture.DbContext.Answers
            .FirstAsync(answer => answer.Question.Id == question.Id && answer.MemberId == previousAnswer.MemberId);

        answer.Value.Should().Be("false");
        answer.MemberId.Should().Be(request.AnswersByMember[0].MemberId);
    }
    
    [Fact]
    public async Task Should_Publish_Event_When_Answers_Are_Updated()
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
        
        var answer = new Answer
        {
            Question = question,
            MemberId = Guid.CreateVersion7(),
            Value = "true"
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.Answers.AddAsync(answer);
        await fixture.DbContext.SaveChangesAsync();

        ValidationService
            .IsValid(Arg.Is<Question>(arg => arg.Id == question.Id), Arg.Any<string?>())
            .Returns(true);
        
        #endregion
        
        var request = new UpdateAnswers
        {
            SurveyId = survey.Id,
            PartyId = Guid.CreateVersion7(),
            AnswersByMember = [(Guid.CreateVersion7(), new (Guid, string?)[1] { (question.Id, "false") })]
        };
        
        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<SurveyAnswered>()
            .Id.Should().NotBeEmpty();
    }
}