using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models.DTOs;
using Quiz_API.Services;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    private readonly ICardService _cardService;
    public CardController(ICardService cardService)
    {
      _cardService = cardService;
    }

    [HttpGet]
    public IEnumerable<CardRespDto> GetCards()
    {
      return _cardService.GetCards();
    }

    [HttpPost]
    public CardRespDto CreateCard (
      [FromBody] CreateCardModel createCardModel,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      return _cardService.CreateCard(createCardModel, authHeader);
    }

    [HttpGet("{id}")]
    public CardRespDto Get(string id)
    {
      return _cardService.GetCardById(id);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateCard(
      string id, 
      UpdateCardDto cardUpdates,
      [FromHeader(Name = "Authorization")] string authHeader
      )
    {
      _cardService.UpdateCard(id, cardUpdates, authHeader);
      return Ok();
    }
  

  }
}
