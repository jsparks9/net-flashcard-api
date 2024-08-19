namespace Quiz_API.Models.DTOs
{
  public class CreateCardModel
  {
    public string QuizText { get; set; }
    public string[] Answers { get; set; }
    public string Image { get; set; }
  }
}
