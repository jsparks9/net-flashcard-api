using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models;
using Quiz_API.Models.DTOs;
using Quiz_API.Services;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DeckController : ControllerBase
  {
    private readonly IDeckService _deckService;
    public DeckController(IDeckService deckService)
    {
      _deckService = deckService;
    }

    [HttpGet]
    public IEnumerable<DeckRespDto> GetDecks()
    {
      return _deckService.GetDecks();
    }

    [HttpGet("user/{username}")]
    public IEnumerable<DeckRespDto> GetDecksByUsername(string username)
    {
      return _deckService.GetDecksByUsername(username);
    }

    [HttpGet("getmydecks")]
    public IEnumerable<DeckRespDto> GetMyDecks([FromHeader(Name = "Authorization")] string authHeader)
    {
      return _deckService.GetMyDecks(authHeader);
    }


    [HttpGet("{id}")]
    public DeckRespDto GetDeckById(string id)
    {
      return _deckService.GetDeckById(id);
    }

    [HttpPost]
    public DeckRespDto CreateDeck(
      [FromBody] CreateDeckModel createDeckModel,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      return _deckService.CreateDeck(createDeckModel, authHeader);
    }

    [HttpPost("deck/{id}")]
    public CardRespDto AddCardToDeck(
      string id,
      [FromBody] CreateCardModel createCardModel,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      return _deckService.AddCardToDeck(id, createCardModel, authHeader);
    }

    [HttpDelete("deck/{deckId}/card/{cardId}")]
    public IActionResult RemoveCardFromDeck(
      string deckId,
      string cardId,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      _deckService.RemoveCardFromDeck(deckId, cardId, authHeader);
      return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateDeck (
      string id,
      [FromBody] UpdateDeckDto updateDeckDto,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      _deckService.UpdateDeck(id, updateDeckDto, authHeader);
      return NoContent();
    }

  }
}
