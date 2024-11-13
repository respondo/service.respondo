using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Survey;
using Respondo.Core.Surveys.Contracts;

namespace Respondo.Api.Controllers;

public partial class OccasionController
{
    [HttpPost("{occasionId:guid}/survey")]
    public async Task<IActionResult> CreateSurvey([FromRoute] Guid occasionId, [FromBody] CreateSurveyRequest model,
        CancellationToken cancellationToken)
    {
        var request = model.ToRequest(occasionId, User.GetProfileId());
        
        await _bus.PublishAsync(request);

        return Accepted();
    }

    [HttpGet("{occasionId:guid}/survey")]
    public async Task<IActionResult> GetSurvey([FromRoute] Guid occasionId, CancellationToken cancellationToken)
    {
        var request = new GetSurvey { OccasionId = occasionId };

        var response = await _bus.InvokeAsync<GetSurveyResponse?>(request, cancellationToken);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost("{occasionId:guid}/survey/{surveyId:guid}/question")]
    public async Task<IActionResult> AddQuestionToSurvey([FromRoute] Guid occasionId, [FromRoute] Guid surveyId,
        [FromBody] AddQuestionRequest model)
    {
        var request = model.ToRequest(surveyId, User.GetProfileId());

        await _bus.PublishAsync(request);

        return Accepted();
    }
}