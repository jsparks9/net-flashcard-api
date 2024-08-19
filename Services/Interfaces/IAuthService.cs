using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Quiz_API.Models;
using Quiz_API.Models.DTOs;

namespace Quiz_API.Services
{
  public interface IAuthService
  {
    Task<string> LoginAsync(LoginModel loginModel);
    Task<string> CreateUserAsync(CreateUserModel createUserModel);
    AppUser GetUserFromToken(string token);
    ClaimsPrincipal ValidateToken(string token);
    JwtSecurityToken ReadToken(string token);
  }
}
