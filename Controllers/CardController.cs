using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models;
using Quiz_API.Models.DTOs;
using Quiz_API.Services;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    private ApplicationDbContext _context;
    private readonly IAuthService _authService;
    public CardController(ApplicationDbContext context, IAuthService authService)
    {
      _context = context;
      _authService = authService;
    }

    [HttpGet]
    public IEnumerable<Card> GetCards()
    {
      return _context.Cards.ToList();
    }

    [HttpPost]
    public IActionResult CreateCard([FromBody] CreateCardModel createCardModel)
    {
      if (createCardModel == null || createCardModel.Answers == null || createCardModel.Answers.Length == 0)
      {
        return BadRequest("Invalid client request");
      }

      var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Authorization token is missing or invalid.");

      var appUser = _authService.GetUserFromToken(token);
      if (appUser == null)
        return Unauthorized();

      var newCard = new Card
      {
        CardId = Guid.NewGuid(),
        QuizText = createCardModel.QuizText,
        Answers = createCardModel.Answers[0] // TODO: save ans properly and add image
      };

      _context.Cards.Add(newCard);
      _context.SaveChanges();

      return Ok("Card created successfully");
    }

  [HttpGet("{id}")]
    public Card Get(string id)
    {
      return _context.Cards.Find(id);
    }

    [HttpPut]
    public IActionResult UpdateCard(string id, Card card)
    {
      try
      {
        if (new Guid(id) != card.CardId)
          return StatusCode(StatusCodes.Status400BadRequest);
        _context.Cards.Update(card);
        _context.SaveChanges();
        return StatusCode(StatusCodes.Status200OK);
      }
      catch
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  

  }
}
