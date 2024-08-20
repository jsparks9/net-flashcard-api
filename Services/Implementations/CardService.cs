using Quiz_API.Models.DTOs;
using Quiz_API.Models;


namespace Quiz_API.Services
{
  public class CardService : ICardService
  {
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;

    public CardService(
      ApplicationDbContext context,
      IAuthService authService
      )
    {
      _context = context;
      _authService = authService;
    }

    public IEnumerable<CardRespDto> GetCards()
    {
      return _context.Cards
        .Join(_context.UserAuths,
            card => card.UserId,
            user => user.UserId,
            (card, user) => new CardRespDto(card, user.Username))
        .ToList();
    }

    public CardRespDto GetCardById(string id)
    {
      var cardId = new Guid(id);

      var result = _context.Cards
          .Where(c => c.CardId == cardId)
          .Join(_context.UserAuths,
              card => card.UserId,
              user => user.UserId,
              (card, user) => new CardRespDto(card, user.Username))
          .FirstOrDefault();

      if (result == null)
          throw new Exception("Not Found");

      return result;
    }

    public CardRespDto CreateCard(CreateCardModel createCardModel, string authHeader)
    {
      if (createCardModel == null || createCardModel.Answer == null || createCardModel.Answer.Length == 0)
        throw new ArgumentException("Invalid client request");

      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null)
        throw new Exception("500");

      var newCard = new Card
      {
        CardId = Guid.NewGuid(),
        QuizText = createCardModel.QuizText,
        Answer = createCardModel.Answer,
        UserId = userInfo.UserId,
      };

      _context.Cards.Add(newCard);
      _context.SaveChanges();

      string username = _context.UserAuths.FirstOrDefault(u => u.UserId == userInfo.UserId).Username;

      return new CardRespDto(newCard, username);
    }

    public void UpdateCard(string id, UpdateCardDto cardUpdates, string authHeader)
    {
      if (cardUpdates == null)
        throw new ArgumentException("Invalid client request");

      var card = _context.Cards.FirstOrDefault(c => c.CardId == new Guid(id));
      if (card == null)
        throw new Exception("Not Found!"); // TODO: replace with 404 type exception

      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);

      if (userInfo == null || card.UserId != userInfo.UserId)
        throw new UnauthorizedAccessException();

      if (cardUpdates.Answer != null && cardUpdates.Answer.Length > 0)
        card.Answer = cardUpdates.Answer;
      if (cardUpdates.QuizText != null && cardUpdates.QuizText.Length > 0)
        card.QuizText = cardUpdates.QuizText;
      if (cardUpdates.Image != null && cardUpdates.Image.Length > 0)
        card.Image = cardUpdates.Image; 


      _context.Cards.Update(card);
      _context.SaveChanges();

      return;
    }
  }
}
