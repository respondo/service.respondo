using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Extensions;
using Respondo.Core.Surveys.Persistence;

namespace Respondo.Core.Surveys;

public sealed record GetSurveyHandler
{
    private readonly SurveysDbContext _db;

    public GetSurveyHandler(SurveysDbContext db)
    {
        _db = db;
    }

    public async Task<GetSurveyResponse?> Handle(GetSurvey request, CancellationToken cancellationToken)
    {
        var query = _db.Surveys
            .AsNoTracking()
            .Include(survey => survey.Questions)
            .Where(survey => survey.OccasionId == request.OccasionId)
            .Select(survey => new GetSurveyResponse
            {
                Id = survey.Id,
                Title = survey.Title,
                Questions = survey.Questions.Select(question => new GetSurveyResponse.Question
                {
                    Id = question.Id,
                    Type = question.GetType().Name,
                    Statement = question.Statement,
                    Required = question.Required,
                    Options = question.RetrieveOptions()
                })
            });

        var survey = await query.FirstOrDefaultAsync(cancellationToken);

        return survey;
    }
}