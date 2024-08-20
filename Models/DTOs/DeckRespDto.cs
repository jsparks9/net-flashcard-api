
namespace Quiz_API.Models.DTOs
{
  public class DeckRespDto
  {
    public Guid DeckId {  get; set; }
    public string User { get; set; }
    public string DeckName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CardRespDto> Cards { get; set; }

    public DeckRespDto(QuizDeck deck, string user)
    {
      DeckId = deck.DeckId;
      User = user;
      DeckName = deck.DeckName;
      Description = deck.Description;
      CreatedAt = deck.CreatedAt;
      Cards = deck.Cards.Select(card => new CardRespDto(card, user)).ToList();
    }

  }
}
