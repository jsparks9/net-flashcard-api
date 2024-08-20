using Quiz_API.Models.DTOs;
using Quiz_API.Models;


namespace Quiz_API.Services
{
  public class DeckService : IDeckService
  {
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;

    public DeckService(
      ApplicationDbContext context,
      IAuthService authService
      )
    {
      _context = context;
      _authService = authService;
    }

    public IEnumerable<DeckRespDto> GetDecks()
    {
      return _context.QuizDecks
        .Select(deck => new DeckRespDto(
            deck,
            _context.UserAuths
                .Where(user => user.UserId == deck.UserId)
                .Select(user => user.Username)
                .FirstOrDefault()
        ))
        .ToList();
    }

    public DeckRespDto CreateDeck(CreateDeckModel createDeckModel, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null)
        throw new Exception("500");
      string username = userInfo.Username;

      var newCards = createDeckModel.Cards?.Select(cardModel => new Card
      {
        CardId = new Guid(),
        QuizText = cardModel.QuizText,
        Answer = cardModel.Answer,
        Image = cardModel.Image,
        UserId = userInfo.UserId
      }).ToList();

      if (newCards != null && newCards.Count > 0)
      {
        _context.Cards.AddRange(newCards);
        _context.SaveChanges();
      }

      var newDeck = new QuizDeck
      {
        DeckName = createDeckModel.DeckName,
        Description = createDeckModel.Description,
        Cards = newCards,
        UserId = userInfo.UserId
      };

      _context.QuizDecks.Add(newDeck);
      _context.SaveChanges();

      return new DeckRespDto(newDeck, username);
    }



  }
}
