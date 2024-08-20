using Quiz_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Quiz_API.Services
{
  public interface IJwtService
  {
    ClaimsPrincipal ValidateToken(string token);
    JwtSecurityToken ReadToken(string token);
    string GenerateJwtToken(UserAuth userAuth);
  }
}
