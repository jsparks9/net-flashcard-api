using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models.DTOs;
using Quiz_API.Services;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DeckController : ControllerBase
  {
    private readonly IDeckService _deckService;
    public DeckController(IDeckService deckService) {
      _deckService = deckService;
    }

    [HttpGet]
    public IEnumerable<DeckRespDto> GetDecks()
    {
      return _deckService.GetDecks();
    }

    [HttpPost]
    public DeckRespDto CreateDeck(
      CreateDeckModel createDeckModel,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      return _deckService.CreateDeck(createDeckModel, authHeader);
    }
  }
}
