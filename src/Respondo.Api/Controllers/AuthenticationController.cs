using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Models;
using Respondo.Core.Identity.Contracts;
using Respondo.Core.Identity.Contracts.Entities;
using Wolverine;

namespace Respondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationController(IMessageBus bus, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _bus = bus;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model, CancellationToken cancellationToken)
    {
        var result = await _bus.InvokeAsync<IdentityResult>(model.ToRequest(), cancellationToken);

        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            return Problem("Unable to create user. Please contact support.");
        }
        
        await _signInManager.SignInAsync(user, true);

        return Redirect("/");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            ModelState.AddModelError("LoginError", "Invalid login attempt. Check email or password.");
            return BadRequest(ModelState);
        }

        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
        {
            ModelState.AddModelError("LoginError", "Invalid login attempt. Check email or password.");
            return BadRequest(ModelState);
        }
        
        await _signInManager.SignInAsync(user, true);

        return Redirect("/");
    }

    [Authorize]
    [HttpGet("authenticate")]
    public IActionResult Authenticate()
    {
        return Ok();
    }
}