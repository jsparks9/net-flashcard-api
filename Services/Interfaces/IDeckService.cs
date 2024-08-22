using Microsoft.AspNetCore.Mvc;
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
    CardRespDto AddCardToDeck(string id, CreateCardModel createCardModel, string authHeader);
    void RemoveCardFromDeck(string deckId, string cardId, string authHeader);
    void UpdateDeck(string id, UpdateDeckDto updateDeckDto, string authHeader);
    void DeleteDeck(string id, string type, string authHeader);
  }
}
