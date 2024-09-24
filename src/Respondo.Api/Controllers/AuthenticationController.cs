using Microsoft.AspNetCore.Mvc;
using Respondo.Api.ViewModels;

namespace Respondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}