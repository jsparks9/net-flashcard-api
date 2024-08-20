namespace Quiz_API.Models.DTOs
{
  public class CreateCardModel
  {
    public string? CardId { get; set; }
    public string QuizText { get; set; }
    public string Answer { get; set; }
    public string Image { get; set; }
  }
}
