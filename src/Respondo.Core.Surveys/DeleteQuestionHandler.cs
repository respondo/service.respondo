using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record DeleteQuestionHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;
    private readonly ILogger<DeleteQuestionHandler> _logger;

    public DeleteQuestionHandler(IMessageContext context, SurveysDbContext db, ILogger<DeleteQuestionHandler> logger)
    {
        _context = context;
        _db = db;
        _logger = logger;
    }

    public async Task Handle(DeleteQuestion request, CancellationToken cancellationToken)
    {
        var question = await _db.Questions
            .Where(question => question.Id == request.QuestionId)
            .Where(question => question.Survey.ProfileId == request.ProfileId)
            .Where(question => question.Survey.Id == request.SurveyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (question == null)
        {
            _logger.LogWarning("Unable to find question {Id} in survey {SurveyId}", request.QuestionId, request.SurveyId);
            return;
        }

        _db.Questions.Remove(question);

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted question {Id} from survey {SurveyId}", request.QuestionId, request.SurveyId);

        await _context.PublishAsync(new QuestionDeleted
        {
            Id = request.QuestionId,
            SurveyId = request.SurveyId
        });
    }
}