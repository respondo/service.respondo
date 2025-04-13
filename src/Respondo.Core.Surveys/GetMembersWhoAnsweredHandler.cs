using Microsoft.EntityFrameworkCore;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record GetMembersWhoAnsweredHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;

    public GetMembersWhoAnsweredHandler(IMessageContext context, SurveysDbContext db)
    {
        _context = context;
        _db = db;
    }

    public async Task<GetMembersWhoAnsweredResponse?> Handle(GetMembersWhoAnswered request,
        CancellationToken cancellationToken)
    {
        var query = _db.Answers.AsNoTracking()
            .Where(answer => answer.Question.Survey.Id == request.SurveyId)
            .Select(answer => answer.MemberId);

        return new GetMembersWhoAnsweredResponse()
        {
            Members = await query.ToListAsync(cancellationToken)
        };
    }

}