namespace Quiz_API.Models.DTOs
{
  public class CreateUserModel
  {
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
