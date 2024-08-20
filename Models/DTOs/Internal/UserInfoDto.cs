namespace Quiz_API.Models.DTOs.Internal
{
  public class UserInfoDto
  {
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool? IsEmailConfirmed { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool? IsActive { get; set; }
    public string Username { get; set; }

    public UserInfoDto(AppUser appUser, string username)
    {
      UserId = appUser.UserId;
      FullName = appUser.FullName;
      Email = appUser.Email;
      IsEmailConfirmed = appUser.IsEmailConfirmed;
      DateOfBirth = appUser.DateOfBirth;
      RegistrationDate = appUser.RegistrationDate;
      LastLogin = appUser.LastLogin;
      IsActive = appUser.IsActive;
      Username = username;
    }

    
  }
}
