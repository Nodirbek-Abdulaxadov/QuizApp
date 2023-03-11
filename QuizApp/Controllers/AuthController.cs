using Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;

namespace QuizApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserViewModel viewModel)
    {
        try
        {
            var result = await _userService.CreateUserAsync(viewModel);
            return StatusCode(201, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserViewModel viewModel)
    {
        try
        {
            var result = await _userService.LoginUserAsync(viewModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh-user")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequstViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userService.VerifyAndGenerateTokenAsync(viewModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        if (refreshToken == null)
        {
            return BadRequest();
        }

        await _userService.Logout(refreshToken);
        return NoContent();
    }
}