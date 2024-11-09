using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Respondo.Core.Surveys;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Testing.Unit.Helpers;
using Wolverine;

namespace Respondo.Testing.Unit.Cores.Surveys;

public class AddQuestionTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private readonly AddQuestionHandler _handler = new(fixture.MessageContext, fixture.DbContext,
        Substitute.For<ILogger<AddQuestionHandler>>());

    [Fact]
    public async Task Should_Add_Question_To_Survey()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldAddQuestionToSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new AddQuestion
        {
            SurveyId = survey.Id,
            Type = AddQuestion.EQuestionType.General,
            Statement = "ShouldAddQuestionToSurvey",
            Required = true,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        var question = fixture.DbContext.Questions
            .Include(question => question.Survey)
            .First(question => question.Statement == request.Statement);

        question.Should().NotBeNull();
        question.Survey.Id.Should().Be(survey.Id);
    }

    [Fact]
    public async Task Should_Publish_Event_When_Question_Is_Added()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldPublishEventWhenQuestionIsAdded",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion

        var request = new AddQuestion
        {
            SurveyId = survey.Id,
            Type = AddQuestion.EQuestionType.General,
            Statement = "ShouldPublishEventWhenQuestionIsAdded",
            Required = true,
            ProfileId = survey.ProfileId
        };

        await _handler.Handle(request, CancellationToken.None);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<QuestionAdded>()
            .Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(AddQuestion.EQuestionType.General, typeof(GeneralQuestion))]
    [InlineData(AddQuestion.EQuestionType.Open, typeof(OpenQuestion))]
    [InlineData(AddQuestion.EQuestionType.SingleChoice, typeof(SingleChoiceQuestion))]
    [InlineData(AddQuestion.EQuestionType.MultipleChoice, typeof(MultipleChoiceQuestion))]
    public async Task Should_Add_Correct_Question_Type_To_Survey(AddQuestion.EQuestionType questionType,
        Type expectedType)
    {
        #region Setup
        
        var survey = new Survey
        {
            Title = "ShouldAddCorrectQuestionTypeToSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };
        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();

        #endregion
        
        var request = new AddQuestion
        {
            SurveyId = survey.Id,
            Type = questionType,
            Statement = $"ShouldAddCorrectQuestionTypeToSurvey:{questionType}",
            Required = true,
            ProfileId = survey.ProfileId,
            Options = questionType is AddQuestion.EQuestionType.SingleChoice or AddQuestion.EQuestionType.MultipleChoice
                ? new List<string> { "Option 1", "Option 2" }
                : null
        };

        await _handler.Handle(request, CancellationToken.None);

        var question = await fixture.DbContext.Questions
            .FirstOrDefaultAsync(q => q.Statement == request.Statement);

        question.Should().NotBeNull();
        question.Should().BeOfType(expectedType);
    }
}