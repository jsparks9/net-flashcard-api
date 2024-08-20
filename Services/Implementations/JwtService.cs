using Microsoft.IdentityModel.Tokens;
using Quiz_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz_API.Services
{
  public class JwtService: IJwtService
  {
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _configuration["Jwt:Issuer"],
        ValidAudience = _configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
      };

      try
      {
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        return principal;
      }
      catch
      {
        return null;
      }
    }

    public JwtSecurityToken ReadToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      return tokenHandler.ReadJwtToken(token) as JwtSecurityToken;
    }

    public string GenerateJwtToken(UserAuth userAuth)
    {
      var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAuth.Username),
                new Claim(ClaimTypes.NameIdentifier, userAuth.UserId.ToString())
            };

      var tokenOptions = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(Double.Parse(_configuration["Jwt:ExpryMinutes"])),
          signingCredentials: signinCredentials
      );

      return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
  }
}
