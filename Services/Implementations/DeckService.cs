using Quiz_API.Models.DTOs;
using Quiz_API.Models;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Exceptions;
using System;


namespace Quiz_API.Services
{
  enum Types
  {
    justdeck,
    deckwithunique,
    deckwithcards
  }
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
        .Include(deck => deck.DeckCards) // Eagerly load the Cards
        .ThenInclude(dc => dc.Card)
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
        .Include(deck => deck.DeckCards) // Eagerly load the Cards
        .ThenInclude(dc => dc.Card)
        .Select(deck => new DeckRespDto(deck, username))
        .ToList();
    }

    public IEnumerable<DeckRespDto> GetMyDecks(string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to get own decks.");
      var userId = userInfo.UserId;
      if (userId == null)
        return Enumerable.Empty<DeckRespDto>();

      return _context.QuizDecks
        .Where(deck => deck.UserId == userId)
        .Include(deck => deck.DeckCards) // Eagerly load the Cards
        .ThenInclude(dc => dc.Card)
        .Select(deck => new DeckRespDto(deck, userInfo.Username))
        .ToList();
    }

    public DeckRespDto GetDeckById(string id)
    {
      var deckId = new Guid(id);

      var deck = _context.QuizDecks
        .Where(deck => deck.DeckId == deckId)
        .Include(deck => deck.DeckCards) // Eagerly load the Cards
        .ThenInclude(dc => dc.Card)
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
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to create.");
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

      var newId = Guid.NewGuid();
      int i = 1;

      var newDeck = new QuizDeck
      {
        DeckId = newId,
        DeckName = createDeckModel.DeckName,
        Description = createDeckModel.Description,
        DeckCards = newCards.Select(card => new DeckCard
        {
          CardId = card.CardId,
          DeckId = newId,
          OrderIndex = i++
        }).ToList(),
        UserId = userInfo.UserId
      };

      _context.QuizDecks.Add(newDeck);
      _context.SaveChanges();

      return new DeckRespDto(newDeck, username);
    }

    public CardRespDto AddCardToDeck(string id, CreateCardModel createCardModel, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to add cards.");

      var deckId = new Guid(id);
      var deck = _context.QuizDecks
        .Include(d => d.DeckCards)
        .ThenInclude(dc => dc.Card)
        .FirstOrDefault(d => d.DeckId == deckId);
      if (deck == null) throw new NotFoundException("Deck not found.");
      if (deck.UserId != userInfo.UserId)
        throw new ForbiddenAccessException("Unauthorized to update this deck");
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
      if (!deck.DeckCards.Any(dc => dc.CardId == cardToAdd.CardId))
      {
        deck.DeckCards.Add(new DeckCard
        {
          DeckId = deck.DeckId,
          CardId = cardToAdd.CardId,
          OrderIndex = deck.DeckCards.Any() ? deck.DeckCards.Max(dc => dc.OrderIndex) + 1 : 1
        });

        _context.SaveChanges(); // Make sure to save changes to the database
      }

      return new CardRespDto(cardToAdd, userInfo.Username);
    }

    public void RemoveCardFromDeck(string deckId, string cardId, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to edit.");

      var deckGuid = new Guid(deckId);
      var cardGuid = new Guid(cardId);

      var deck = _context.QuizDecks
        .Include(d => d.DeckCards)
        .ThenInclude(dc => dc.Card)
        .FirstOrDefault(d => d.DeckId == deckGuid);
      if (deck == null) throw new NotFoundException("Deck not found.");
      if (deck.UserId != userInfo.UserId)
        throw new ForbiddenAccessException("Not authorized to modify this deck.");

      var cardToRemove = deck.DeckCards
        .FirstOrDefault(dc => dc.CardId == cardGuid);

      if (cardToRemove == null)
        throw new NotFoundException("Card not found in deck.");

      deck.DeckCards.Remove(cardToRemove);
      _context.SaveChanges();

      return;
    }

    public void UpdateDeck(string id, UpdateDeckDto updateDeckDto, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to edit.");

      if (id == null || id.Length < 1)
        throw new ArgumentException("Deck ID is not valid.");

      var deckId = new Guid(id);
      var deck = _context.QuizDecks
        .Include(d => d.DeckCards)
        .ThenInclude(dc => dc.Card)
        .FirstOrDefault(d => d.DeckId == deckId);
      if (deck == null) throw new NotFoundException("Deck not found.");
      if (deck.UserId != userInfo.UserId)
        throw new ForbiddenAccessException("Not authorized to modify this deck.");

      if (updateDeckDto.DeckName != null && updateDeckDto.DeckName.Length > 0)
        deck.DeckName = updateDeckDto.DeckName;
      if (updateDeckDto.Description != null && updateDeckDto.Description.Length > 0)
        deck.Description = updateDeckDto.Description;

      _context.QuizDecks.Update(deck);
      _context.SaveChanges();

      return;
    }

    public void DeleteDeck(string id, string type, string authHeader)
    {
      var userInfo = _authService.GetUserInfoFromAuthHeader(authHeader);
      if (userInfo == null) throw new UnauthorizedAccessException("Must be logged in to delete.");

      if (id == null || id.Length < 1)
        throw new ArgumentException("Deck ID is not valid.");

      if (!Enum.TryParse(type, out Types t))
        throw new ArgumentException($"{type} is an invalid parameter.");

      var deckId = new Guid(id);
      var deck = _context.QuizDecks
        .Include(d => d.DeckCards)
        .ThenInclude(dc => dc.Card)
        .FirstOrDefault(d => d.DeckId == deckId);

      if (deck == null) throw new NotFoundException("Deck not found.");

      if (deck.UserId != userInfo.UserId)
        throw new ForbiddenAccessException("Not authorized to delete this deck.");

      switch ((Types)Enum.Parse(typeof(Types), type))
      {
        case Types.justdeck:
          _context.DeckCards.RemoveRange(deck.DeckCards);
          _context.QuizDecks.Remove(deck);
          break;
        case Types.deckwithunique:
          var cardsToDelete = deck.DeckCards
            .Where(dc => dc.Card.DeckCards.Count == 1) // Only associated with this deck
            .Select(dc => dc.Card)
            .ToList();
          _context.DeckCards.RemoveRange(deck.DeckCards);
          _context.Cards.RemoveRange(cardsToDelete);
          _context.QuizDecks.Remove(deck);
          break;
        case Types.deckwithcards:
          var cardsToDelete1 = deck.DeckCards
            .Select(dc => dc.Card)
            .ToList();
          _context.DeckCards.RemoveRange(deck.DeckCards);
          _context.Cards.RemoveRange(cardsToDelete1);
          _context.QuizDecks.Remove(deck);
          break;
      }
      _context.SaveChanges();
    }

  }
}
