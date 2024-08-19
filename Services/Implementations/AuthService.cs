using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Quiz_API.Models.DTOs;
using Quiz_API.Models;


namespace Quiz_API.Services
{
  public class AuthService : IAuthService
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
      var userAuth = await _context.UserAuths
          .FirstOrDefaultAsync(ua => ua.Username == loginModel.Username);

      if (userAuth == null || !VerifyPassword(loginModel.Password, userAuth.UserPwd))
      {
        return null;
      }

      return GenerateJwtToken(userAuth);
    }

    public async Task<string> CreateUserAsync(CreateUserModel createUserModel)
    {
      var existingUser = await _context.AppUsers
          .FirstOrDefaultAsync(u => u.Email == createUserModel.Email);

      if (existingUser != null)
        return "Conflict";

      var existingAuth = await _context.UserAuths
          .FirstOrDefaultAsync(u => u.Username == createUserModel.Username);

      if (existingUser != null)
        return "Conflict";

      var newUser = new AppUser
      {
        UserId = Guid.NewGuid(),
        FullName = createUserModel.FullName,
        Email = createUserModel.Email,
      };

      var newUserAuth = new UserAuth
      {
        UserId = newUser.UserId,
        UserPwd = HashPassword(createUserModel.Password),
        Username = createUserModel.Username,
        AppUser = newUser
      };

      _context.AppUsers.Add(newUser);
      _context.UserAuths.Add(newUserAuth);
      await _context.SaveChangesAsync();

      return "User created successfully";
    }

    public AppUser GetUserFromToken(string token)
    {
      if (string.IsNullOrEmpty(token))
        return null;

      var principal = ValidateToken(token);
      if (principal == null)
        return null;

      return GetUserFromPrincipal(principal).Result;
    }

    public async Task<AppUser> GetUserFromPrincipal(ClaimsPrincipal principal)
    {
      if (principal == null)
        return null;

      var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
      var usernameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

      if (userIdClaim == null || usernameClaim == null)
        return null;

      var userId = new Guid(userIdClaim.Value);
      var username = usernameClaim.Value;

      var userAuth = await _context.UserAuths
          .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.Username == username);

      if (userAuth == null)
        return null;

      var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserId == userAuth.UserId);
      if (user == null)
        return null;

      return user;
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

    private bool VerifyPassword(string inputPassword, string storedPasswordHash)
    {
      string hashedInputPassword = HashPassword(inputPassword);
      return hashedInputPassword == storedPasswordHash;
    }

    private string HashPassword(string password)
    {
      string salt = _configuration["Security:PasswordSalt"];
      using (var sha256 = SHA256.Create())
      {
        var saltedPassword = password + salt;
        byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
        byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
        return Convert.ToBase64String(hashBytes);
      }
    }

    private string GenerateJwtToken(UserAuth userAuth)
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
