using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record GetAnswersByPartyHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;

    public GetAnswersByPartyHandler(IMessageContext context, SurveysDbContext db)
    {
        _context = context;
        _db = db;
    }

    public async Task<GetAnswersByPartyResponse?> Handle(GetAnswersByParty request, CancellationToken cancellationToken)
    {
        var party = await _context.InvokeAsync<GetPartyResponse?>(new GetParty { Id = request.PartyId });

        if (party is null)
        {
            return null;
        }

        // Get the survey for this party's occasion
        var survey = await _db.Surveys
            .AsNoTracking()
            .Include(s => s.Questions)
            .FirstOrDefaultAsync(s => s.OccasionId == party.OccasionId, cancellationToken);

        if (survey is null)
        {
            return null;
        }

        // Extract member IDs
        var memberIds = party.Members.Select(m => m.Id).ToList();

        // Get all answers for the survey questions from this party's members
        var answers = await _db.Answers
            .AsNoTracking()
            .Include(a => a.Question)
            .Where(a => survey.Questions.Select(q => q.Id).Contains(a.Question.Id))
            .Where(a => memberIds.Contains(a.MemberId))
            .ToListAsync(cancellationToken);

        // Group answers by member
        var answersByMember = answers
            .GroupBy(a => a.MemberId)
            .Select(g => new GetAnswersByPartyResponse.MemberAnswers
            {
                MemberId = g.Key,
                Answers = g.Select(a => new GetAnswersByPartyResponse.QuestionAnswer
                {
                    QuestionId = a.Question.Id,
                    QuestionStatement = a.Question.Statement,
                    QuestionType = a.Question.GetType().Name,
                    Answer = a.Value
                })
            })
            .ToList();

        return new GetAnswersByPartyResponse
        {
            SurveyId = survey.Id,
            SurveyTitle = survey.Title,
            AnswersByMember = answersByMember
        };
    }
}
