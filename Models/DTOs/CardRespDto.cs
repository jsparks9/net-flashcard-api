namespace Quiz_API.Models.DTOs
{
  public class CardRespDto
  {
    public Guid CardId { get; set; }
    public string User { get; set; }
    public string QuizText { get; set; }
    public string Answer { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }

    public CardRespDto(Card card, string user) {
      CardId = card.CardId;
      User = user;
      QuizText = card.QuizText;
      Answer = card.Answer;
      Image = card.Image;
      CreatedAt = card.CreatedAt;
    }
  }
}
