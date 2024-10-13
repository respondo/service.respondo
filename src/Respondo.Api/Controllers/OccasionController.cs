using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Occasion;
using Respondo.Core.Occasions.Contracts;
using Wolverine;

namespace Respondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OccasionController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OccasionController(IMessageBus bus)
    {
        _bus = bus;
    }

    [Authorize]
    [HttpGet("{occasionId:guid}")]
    public async Task<IActionResult> GetOccasion([FromRoute] Guid occasionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOccasion([FromBody] CreateOccasionModel model, CancellationToken cancellationToken)
    {
        var request = model.ToRequest(User.GetProfileId());

        var result = await _bus.InvokeAsync<CreateOccasionResponse>(request, cancellationToken);

        if (result.Id != default)
        {
            return Ok(new { result.Id });
        }
        
        return Problem("Unable to create occasion. Please contact support.");
    }

}