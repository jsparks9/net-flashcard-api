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

    [HttpPost("login")]
    public LoginResponseDto Login([FromBody] LoginModel loginModel)
    {
      return _authService.Login(loginModel);
    }

    [HttpPost("register")]
    public LoginResponseDto CreateUser([FromBody] CreateUserModel createUserModel)
    {
      return _authService.CreateUser(createUserModel);
    }
  }
}
