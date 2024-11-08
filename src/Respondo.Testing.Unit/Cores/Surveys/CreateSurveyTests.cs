using FluentAssertions;
using Respondo.Core.Surveys;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Testing.Unit.Helpers;
using Wolverine;

namespace Respondo.Testing.Unit.Cores.Surveys;

public class CreateSurveyTests(UnitFixture<SurveysDbContext> fixture) : IClassFixture<UnitFixture<SurveysDbContext>>
{
    private readonly CreateSurveyHandler _handler = new(fixture.MessageContext, fixture.DbContext);
    
    [Fact]
    public async Task Should_Create_Survey()
    {
        var request = new CreateSurvey
        {
            Title = "ShouldCreateSurvey",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await _handler.Handle(request);
        
        fixture.DbContext.Surveys.Should().Contain(survey => survey.Title == request.Title);
    }
    
    [Fact]
    public async Task Should_Publish_Event_When_Survey_Is_Created()
    {
        var request = new CreateSurvey
        {
            Title = "ShouldPublishEventWhenSurveyIsCreated",
            OccasionId = Guid.CreateVersion7(),
            ProfileId = Guid.CreateVersion7()
        };

        await _handler.Handle(request);

        fixture.MessageContext.Published
            .ShouldHaveMessageOfType<SurveyCreated>()
            .Id.Should().NotBeEmpty();
    }
}