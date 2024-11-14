using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Occasion;
using Respondo.Core.Occasions.Contracts;
using Wolverine;

namespace Respondo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public partial class OccasionController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OccasionController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet]
    public async Task<IActionResult> GetOccasions(CancellationToken cancellationToken)
    {
        var request = new GetOccasions
        {
            ProfileId = User.GetProfileId()
        };

        var result = await _bus.InvokeAsync<GetOccasionsResponse>(request, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{occasionId:guid}")]
    public async Task<IActionResult> GetOccasion([FromRoute] Guid occasionId, CancellationToken cancellationToken)
    {
        var request = new GetOccasion
        {
            Id = occasionId,
            ProfileId = User.GetProfileId()
        };

        var result = await _bus.InvokeAsync<GetOccasionResponse?>(request, cancellationToken);

        if (result is null)
        {
            return NotFound(new { request.Id });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOccasion([FromBody] CreateOccasionModel model, CancellationToken cancellationToken)
    {
        var request = model.ToRequest(User.GetProfileId());

        var result = await _bus.InvokeAsync<CreateOccasionResponse>(request, cancellationToken);

        if (result.Id != default)
        {
            return Ok(result);
        }

        return Problem("Unable to create occasion. Please contact support.");
    }

    [HttpPut("{occasionId:guid}")]
    public async Task<IActionResult> UpdateOccasion([FromBody] UpdateOccasionRequest model, [FromRoute] Guid occasionId)
    {
        var request = model.ToRequest(occasionId, User.GetProfileId());

        await _bus.InvokeAsync(request);

        return Ok();
    }
}