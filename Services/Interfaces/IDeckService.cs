using Quiz_API.Models.DTOs;

namespace Quiz_API.Services
{
  public interface IDeckService
  {
    IEnumerable<DeckRespDto> GetDecks();
    DeckRespDto CreateDeck(CreateDeckModel createDeckModel, string authHeader);
  }
}
