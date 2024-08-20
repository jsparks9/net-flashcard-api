using Quiz_API.Models.DTOs;

namespace Quiz_API.Services
{
  public interface ICardService
  {
    IEnumerable<CardRespDto> GetCards();
    CardRespDto GetCardById(string id);
    CardRespDto CreateCard(CreateCardModel createCardModel, string authHeader);
    void UpdateCard(string id, UpdateCardDto cardUpdates, string authHeader);
  }
}
