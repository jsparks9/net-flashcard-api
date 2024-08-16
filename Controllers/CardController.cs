using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    private ApplicationDbContext _context;
    public CardController(ApplicationDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<Card> GetCards()
    {
      return _context.Cards.ToList();
    }

    [HttpPost]
    public IActionResult AddCard(Card card)
    {
      try
      {
        _context.Cards.Add(card);
        _context.SaveChanges();
        return StatusCode(StatusCodes.Status201Created, card);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
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
      catch (Exception e)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

  }
}
