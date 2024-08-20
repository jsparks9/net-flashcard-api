using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Models
{
  [Table("deck_cards")]
  public class DeckCard
  {
    [Key]
    [Column("deck_id", Order = 0, TypeName = "uniqueidentifier")]
    public Guid DeckId { get; set; }

    [Key]
    [Column("card_id", Order = 1, TypeName = "uniqueidentifier")]
    public Guid CardId { get; set; }

    [Required]
    [Column("order_index", TypeName = "int")]
    public int OrderIndex { get; set; }

    // Navigation properties
    [ForeignKey("DeckId")]
    public virtual QuizDeck QuizDeck { get; set; }

    [ForeignKey("CardId")]
    public virtual Card Card { get; set; }
  }
}
