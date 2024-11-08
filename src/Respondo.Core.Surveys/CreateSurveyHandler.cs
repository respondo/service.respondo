using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record CreateSurveyHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;

    public CreateSurveyHandler(IMessageContext context, SurveysDbContext db)
    {
        _context = context;
        _db = db;
    }


    public async Task Handle(CreateSurvey request)
    {
        var survey = new Survey
        {
            Title = request.Title,
            OccasionId = request.OccasionId,
            ProfileId = request.ProfileId
        };

        await _db.Surveys.AddAsync(survey);

        await _db.SaveChangesAsync();

        await _context.PublishAsync(new SurveyCreated
        {
            Id = survey.Id
        });
    }
}