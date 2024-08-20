using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models.DTOs;
using Quiz_API.Services;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : Controller
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    // POST: api/Auth/Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
      if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
      {
        return BadRequest("Invalid client request");
      }

      var token = await _authService.LoginAsync(loginModel);
      if (token == null)
        return Unauthorized();

      return Ok(new { Token = token });
    }

    // POST: api/Auth/CreateUser
    [HttpPost("register")]
    public IActionResult CreateUser([FromBody] CreateUserModel createUserModel)
    {
      _authService.CreateUser(createUserModel);

      return Created();
    }
  }
}
