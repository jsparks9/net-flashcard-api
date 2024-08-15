using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_API.Models;

namespace Quiz_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    public static List<Card> cards = new List<Card>()
    {
      new Card(){cardId="001", question="A"},
      new Card(){cardId="002", question="B"},
      new Card(){cardId="003", question="C"}
    };

    [HttpGet]
    public List<Card> GetCards()
    {
      return cards;
    }

    [HttpPost]
    public void PostCard()
    {
      cards.Add(new Card() { cardId = "005", question="E" });
    }
  }
}
