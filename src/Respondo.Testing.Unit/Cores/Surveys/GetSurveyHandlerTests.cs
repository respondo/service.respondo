using FluentAssertions;
using Respondo.Core.Surveys;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Testing.Unit.Helpers;

namespace Respondo.Testing.Unit.Cores.Surveys;

public class GetSurveyHandlerTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{

    private readonly GetSurveyHandler _handler = new(fixture.DbContext);

    [Fact]
    public async Task Should_Get_Survey()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldGetSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.SaveChangesAsync();
        
        #endregion
        
        var request = new GetSurvey
        {
            OccasionId = survey.OccasionId
        };
        
        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response?.Id.Should().Be(survey.Id);
        response?.Title.Should().Be("ShouldGetSurvey");
    }
    
    [Fact]
    public async Task Should_Get_Survey_With_Questions()
    {
        #region Setup

        var survey = new Survey
        {
            Title = "ShouldGetSurveyWithQuestions",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7(),
        };

        var question = new GeneralQuestion
        {
            Statement = "ShouldGetSurveyWithQuestions",
            Required = true,
            Survey = survey
        };

        await fixture.DbContext.Surveys.AddAsync(survey);
        await fixture.DbContext.Questions.AddAsync(question);
        await fixture.DbContext.SaveChangesAsync();

        #endregion
        
        var request = new GetSurvey
        {
            OccasionId = survey.OccasionId
        };
        
        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response?.Questions.Should().HaveCount(1);
        response?.Questions.First().Id.Should().Be(question.Id);
        response?.Questions.First().Statement.Should().Be("ShouldGetSurveyWithQuestions");
        response?.Questions.First().Required.Should().BeTrue();
    }
}