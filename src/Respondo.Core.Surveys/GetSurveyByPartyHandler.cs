using Microsoft.EntityFrameworkCore;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Extensions;
using Respondo.Core.Surveys.Persistence;

namespace Respondo.Core.Surveys;

public sealed record GetSurveyByPartyHandler
{
    private readonly SurveysDbContext _db;

    public GetSurveyByPartyHandler(SurveysDbContext db)
    {
        _db = db;
    }

    public async Task<GetSurveyByPartyResponse?> Handle(GetSurvey request, CancellationToken cancellationToken)
    {
        var query = _db.Surveys
            .AsNoTracking()
            .Include(survey => survey.Questions)
            .Where(survey => survey.OccasionId == request.OccasionId)
            .Select(survey => new GetSurveyByPartyResponse
            {
                Id = survey.Id,
                Title = survey.Title,
                Questions = survey.Questions.Select(question => new GetSurveyByPartyResponse.Question
                {
                    Id = question.Id,
                    Statement = question.Statement,
                    Required = question.Required,
                    Options = question.RetrieveOptions()
                })
            });

        var survey = await query.FirstOrDefaultAsync(cancellationToken);

        return survey;
    }
}