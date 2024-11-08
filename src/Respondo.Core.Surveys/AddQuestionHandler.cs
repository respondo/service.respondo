using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record AddQuestionHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;
    private readonly ILogger<AddQuestionHandler> _logger;

    public AddQuestionHandler(IMessageContext context, SurveysDbContext db, ILogger<AddQuestionHandler> logger)
    {
        _context = context;
        _db = db;
        _logger = logger;
    }

    public async Task Handle(AddQuestion request, CancellationToken cancellationToken)
    {
        var survey = await _db.Surveys
            .Where(survey => survey.Id == request.SurveyId)
            .Where(survey => survey.ProfileId == request.ProfileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Unable to find survey {SurveyId} for profile {ProfileId}", request.SurveyId,
                request.ProfileId);

            return;
        }

        Question? question = request.Type switch
        {
            AddQuestion.EQuestionType.General => new GeneralQuestion
            {
                Statement = request.Statement,
                Required = request.Required,
                Survey = survey
            },
            AddQuestion.EQuestionType.Open => new OpenQuestion
            {
                Statement = request.Statement,
                Required = request.Required,
                Survey = survey
            },
            AddQuestion.EQuestionType.SingleChoice when request.Options is { Count: > 0 } => new SingleChoiceQuestion
            {
                Statement = request.Statement,
                Required = request.Required,
                Survey = survey,
                Options = request.Options
            },
            AddQuestion.EQuestionType.MultipleChoice when request.Options is { Count: > 0 } => new
                MultipleChoiceQuestion
                {
                    Statement = request.Statement,
                    Required = request.Required,
                    Survey = survey,
                    Options = request.Options
                },
            _ => default
        };

        if (question is null && request.Options is null or { Count: 0 })
        {
            _logger.LogError("Question type {Type} requires options, but none were provided", request.Type);
            return;
        }

        if (question is null && Enum.IsDefined(request.Type) == false)
        {
            _logger.LogError("Unknown question type {Type}", request.Type);
            return;
        }
        
        if (question is null)
        {
            _logger.LogWarning("Unable to create question of type {Type}", request.Type);
            return;
        }

        await _db.Questions.AddAsync(question, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        await _context.PublishAsync(new QuestionAdded
        {
            Id = question.Id
        });
    }
}