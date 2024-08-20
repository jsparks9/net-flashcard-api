using Quiz_API.Models.DTOs;
using Quiz_API.Models;
using Microsoft.EntityFrameworkCore;


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
        .Include(deck => deck.Cards) // Eagerly load the Cards
        .Select(deck => new DeckRespDto(
            deck,
            _context.UserAuths
                .Where(user => user.UserId == deck.UserId)
                .Select(user => user.Username)
                .FirstOrDefault()
        ))
        .ToList();
    }

    public IEnumerable<DeckRespDto> GetDecksByUsername(string username)
    {
      var userId = _context.UserAuths
        .Where(user => user.Username == username)
        .Select(user => user.UserId)
        .FirstOrDefault();

      if (userId == null)
        return Enumerable.Empty<DeckRespDto>();

      return _context.QuizDecks
        .Where(deck => deck.UserId == userId)
        .Include(deck => deck.Cards) // Eagerly load the Cards
        .Select(deck => new DeckRespDto(deck, username))
        .ToList();
    }

    public IEnumerable<DeckRespDto> GetMyDecks(string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null)
        throw new Exception("Not Auth'd");
      var userId = userInfo.UserId;
      if (userId == null)
        return Enumerable.Empty<DeckRespDto>();

      return _context.QuizDecks
        .Where(deck => deck.UserId == userId)
        .Include(deck => deck.Cards) // Eagerly load the Cards
        .Select(deck => new DeckRespDto(deck, userInfo.Username))
        .ToList();
    }

    public DeckRespDto GetDeckById(string id)
    {
      var deckId = new Guid(id);

      var deck = _context.QuizDecks
        .Where(deck => deck.DeckId == deckId)
        .Include(deck => deck.Cards) // Eagerly load the Cards
        .Select(deck => new DeckRespDto(
            deck,
            _context.UserAuths
                .Where(user => user.UserId == deck.UserId)
                .Select(user => user.Username)
                .FirstOrDefault()
        ))
        .FirstOrDefault();

      return deck;
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

    public CardRespDto AddCardToDeck(string id, CreateCardModel createCardModel, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null)
        throw new Exception("500");

      var deckId = new Guid(id);
      var deck = _context.QuizDecks.Include(d => d.Cards).FirstOrDefault(d => d.DeckId == deckId);
      if (deck == null) throw new Exception("Deck not found.");
      if (deck.UserId != userInfo.UserId) throw new Exception("Not auth");
      Card cardToAdd = null;

      if (createCardModel.CardId != null)
      {
        var cardId = new Guid(createCardModel.CardId);
        var existingCard = _context.Cards.FirstOrDefault(c => c.CardId == cardId);
        if (existingCard != null && existingCard.UserId == userInfo.UserId && 
          existingCard.QuizText == createCardModel.QuizText && existingCard.Answer == createCardModel.Answer
          && existingCard.Image == createCardModel.Image)
        {
          cardToAdd = existingCard;
        }
      }

      if (cardToAdd == null)
      {
        cardToAdd = new Card()
        {
          CardId = Guid.NewGuid(),
          UserId = userInfo.UserId,
          QuizText = createCardModel.QuizText,
          Answer = createCardModel.Answer,
          Image = createCardModel.Image
        };
        _context.Cards.Add(cardToAdd);
      }
      deck.Cards.Add(cardToAdd);
      _context.SaveChanges();

      return new CardRespDto(cardToAdd, userInfo.Username);
    }

    public void RemoveCardFromDeck(string deckId, string cardId, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new Exception("500");

      var deckGuid = new Guid(deckId);
      var cardGuid = new Guid(cardId);

      var deck = _context.QuizDecks.Include(d => d.Cards).FirstOrDefault(d => d.DeckId == deckGuid);
      if (deck == null) throw new Exception("Deck not found.");
      if (deck.UserId != userInfo.UserId) throw new Exception("Not authorized");

      var cardToRemove = deck.Cards.FirstOrDefault(c => c.CardId == cardGuid);
      if (cardToRemove == null)
        throw new Exception("Card not found in deck.");

      deck.Cards.Remove(cardToRemove);
      _context.SaveChanges();

      return;
    }


  }
}
