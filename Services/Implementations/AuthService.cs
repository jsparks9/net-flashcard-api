using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Quiz_API.Models.DTOs;
using Quiz_API.Models;
using Quiz_API.Models.DTOs.Internal;
using Quiz_API.Exceptions;


namespace Quiz_API.Services
{
  public class AuthService : IAuthService
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, IJwtService jwtService)
    {
      _context = context;
      _configuration = configuration;
      _jwtService = jwtService;
    }

    public LoginResponseDto Login(LoginModel loginModel)
    {
      if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
        throw new ArgumentException("Username and password required.");

      var userAuth = _context.UserAuths
          .FirstOrDefault(ua => ua.Username == loginModel.Username);

      if (userAuth == null || !VerifyPassword(loginModel.Password, userAuth.UserPwd))
        throw new UnauthorizedAccessException("Invalid username or password.");

      var fullName = _context.AppUsers
        .FirstOrDefault(u => u.UserId == userAuth.UserId)?.FullName;

      var token = _jwtService.GenerateJwtToken(userAuth);
      if (token == null) throw new Exception();

      return new LoginResponseDto
      {
        Username = userAuth.Username,
        FullName = fullName,
        Token = token
      };
    }

    public LoginResponseDto CreateUser(CreateUserModel createUserModel)
    {
      if (createUserModel == null || string.IsNullOrEmpty(createUserModel.Username) || string.IsNullOrEmpty(createUserModel.Password)
    || string.IsNullOrEmpty(createUserModel.Email) || string.IsNullOrEmpty(createUserModel.FullName))
      {
        throw new ArgumentException("Invalid client request. All fields are required.");
      }

      var existingUser = _context.AppUsers
          .FirstOrDefault(u => u.Email == createUserModel.Email);

      if (existingUser != null)
        throw new ConflictException("A user with this email already exists.");

      var existingAuth = _context.UserAuths
          .FirstOrDefault(u => u.Username == createUserModel.Username);

      if (existingUser != null)
        throw new ConflictException("A user with this username already exists.");

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
      _context.SaveChanges();

      var token = _jwtService.GenerateJwtToken(newUserAuth);
      if (token == null) throw new Exception();

      return new LoginResponseDto
      {
        Username = newUserAuth.Username,
        FullName = newUser.FullName,
        Token = token
      };
    }

    public UserInfoDto GetUserInfoFromAuthHeader(string authHeader)
    {
      var token = authHeader.Split(" ").Last();
      if (string.IsNullOrEmpty(token))
        throw new UnauthorizedAccessException("Authorization token is missing or invalid.");

      var principal = _jwtService.ValidateToken(token);
      if (principal == null)
        throw new UnauthorizedAccessException("Invalid or expired authorization token.");

      var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
      var usernameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

      if (userIdClaim == null || usernameClaim == null)
        throw new UnauthorizedAccessException("Invalid or expired authorization token.");

      var userId = new Guid(userIdClaim.Value);
      var username = usernameClaim.Value;

      var userInfo = (from u in _context.AppUsers
                      join ua in _context.UserAuths on u.UserId equals ua.UserId
                      where ua.UserId == userId && ua.Username == username
                      select new UserInfoDto(u, username)).FirstOrDefault();
      Console.WriteLine(userInfo);
      
      return userInfo;
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

    
  }
}
