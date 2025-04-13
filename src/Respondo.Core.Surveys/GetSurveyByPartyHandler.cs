using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Extensions;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record GetSurveyByPartyHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;

    public GetSurveyByPartyHandler(IMessageContext context, SurveysDbContext db)
    {
        _context = context;
        _db = db;
    }

    public async Task<GetSurveyByPartyResponse?> Handle(GetSurveyByParty request, CancellationToken cancellationToken)
    {
        var party = await _context.InvokeAsync<GetPartyResponse?>(new GetParty { Id = request.PartyId });

        if (party is null)
        {
            return null;
        }
            
        var query = _db.Surveys
            .AsNoTracking()
            .Include(survey => survey.Questions)
            .Where(survey => survey.OccasionId == party.OccasionId)
            .Select(survey => new GetSurveyByPartyResponse
            {
                Id = survey.Id,
                Title = survey.Title,
                Questions = survey.Questions.Select(question => new GetSurveyByPartyResponse.Question
                {
                    Id = question.Id,
                    Statement = question.Statement,
                    Required = question.Required,
                    Type = question.GetType().Name,
                    Options = question.RetrieveOptions()
                })
            });

        var survey = await query.FirstOrDefaultAsync(cancellationToken);

        return survey;
    }
}