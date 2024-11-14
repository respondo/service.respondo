using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record UpdateSurveyHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;
    private readonly ILogger<UpdateSurveyHandler> _logger;

    public UpdateSurveyHandler(IMessageContext context, SurveysDbContext db, ILogger<UpdateSurveyHandler> logger)
    {
        _context = context;
        _db = db;
        _logger = logger;
    }

    public async Task Handle(UpdateSurvey request, CancellationToken cancellationToken)
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

        if (request.Title is not { Length: > 0 })
        {
            _logger.LogWarning("Cannot update survey {Id} with empty title", request.Id);
            return;
        }

        survey.Title = request.Title;

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated survey {Id}", request.Id);

        await _context.PublishAsync(new SurveyUpdated
        {
            Id = request.Id
        });
    }
}