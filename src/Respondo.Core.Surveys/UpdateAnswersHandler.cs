using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Surveys.Contracts;
using Respondo.Core.Surveys.Entities;
using Respondo.Core.Surveys.Persistence;
using Respondo.Core.Surveys.Services;
using Wolverine;

namespace Respondo.Core.Surveys;

public sealed record UpdateAnswersHandler
{
    private readonly IMessageContext _context;
    private readonly SurveysDbContext _db;
    private readonly IAnswerValidationService _answerValidation;
    private readonly ILogger<UpdateAnswersHandler> _logger;


    public UpdateAnswersHandler(IMessageContext context, SurveysDbContext db, IAnswerValidationService answerValidation,
        ILogger<UpdateAnswersHandler> logger)
    {
        _context = context;
        _db = db;
        _answerValidation = answerValidation;
        _logger = logger;
    }

    public async Task Handle(UpdateAnswers request, CancellationToken cancellationToken)
    {
        var survey = await _db.Surveys
            .Include(survey => survey.Questions)
            .FirstOrDefaultAsync(survey => survey.Id == request.SurveyId, cancellationToken: cancellationToken);

        if (survey is null)
        {
            _logger.LogWarning("Unable to find survey {SurveyId}", request.SurveyId);
            return;
        }

        foreach (var (memberId, answers) in request.AnswersByMember)
        {
            //TODO: check if member exists
            foreach (var (questionId, answerToQuestion) in answers)
            {
                var question = survey.Questions.Find(question => question.Id == questionId);

                var previousAnswer = await _db.Answers
                    .Where(answer => answer.Question.Id == questionId)
                    .Where(answer => answer.MemberId == memberId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (previousAnswer is not null)
                {
                    _db.Answers.Remove(previousAnswer);
                }
                
                if (question is null)
                {
                    _logger.LogWarning(
                        "Unable to find question {QuestionId}, cancelling answering of survey {SurveyId}", questionId,
                        survey.Id);
                    return;
                }

                if (_answerValidation.IsValid(question, answerToQuestion))
                {
                    var answer = new Answer
                    {
                        Question = question,
                        MemberId = memberId,
                        Value = answerToQuestion
                    };

                    await _db.Answers.AddAsync(answer, cancellationToken);
                }
                else
                {
                    _logger.LogWarning(
                        "Answer to question {QuestionId} is invalid, cancelling answering of survey {SurveyId}",
                        questionId, request.SurveyId);
                    return;
                }
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        await _context.PublishAsync(new SurveyAnswered
        {
            Id = request.SurveyId,
            PartyId = request.PartyId
        });
    }
}