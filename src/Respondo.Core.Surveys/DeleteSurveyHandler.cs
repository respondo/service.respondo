using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record DeleteSurveyHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;
    private readonly ILogger<DeleteSurveyHandler> _logger;

    public DeleteSurveyHandler(IMessageContext context, SurveysDbContext db, ILogger<DeleteSurveyHandler> logger)
    {
        _context = context;
        _db = db;
        _logger = logger;
    }

    public async Task Handle(DeleteSurvey request, CancellationToken cancellationToken)
    {
        var survey = await _db.Surveys
            .Where(survey => survey.Id == request.Id)
            .Where(survey => survey.ProfileId == request.ProfileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Unable to find survey {SurveyId} for profile {ProfileId}", request.Id,
                request.ProfileId);
            return;
        }

        _db.Surveys.Remove(survey);

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted survey {Id}", request.Id);

        await _context.PublishAsync(new SurveyDeleted
        {
            Id = request.Id
        });
    }
}