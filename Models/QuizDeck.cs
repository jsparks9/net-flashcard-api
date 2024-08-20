using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Models
{
  [Table("quiz_deck")]
  public class QuizDeck
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("deck_id", TypeName = "uniqueidentifier")]
    public Guid DeckId { get; set; } = Guid.NewGuid();

    [Column("deck_name", TypeName = "nvarchar(100)")]
    public string DeckName { get; set; }

    [ForeignKey("AppUser")]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("description", TypeName = "nvarchar(max)")]
    public string Description { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation property
    public virtual AppUser AppUser { get; set; }
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
  }
}
