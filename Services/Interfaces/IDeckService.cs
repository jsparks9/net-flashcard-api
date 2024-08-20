using Quiz_API.Models.DTOs;

namespace Quiz_API.Services
{
  public interface IDeckService
  {
    IEnumerable<DeckRespDto> GetDecks();
    IEnumerable<DeckRespDto> GetDecksByUsername(string username);
    IEnumerable<DeckRespDto> GetMyDecks(string authHeader);
    DeckRespDto GetDeckById(string id);
    DeckRespDto CreateDeck(CreateDeckModel createDeckModel, string authHeader);
  }
}
