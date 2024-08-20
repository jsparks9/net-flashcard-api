using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Models
{
  [Table("quiz_card")]
  public class Card
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("card_id", TypeName = "uniqueidentifier")]
    public Guid CardId { get; set; } = Guid.NewGuid();

    [ForeignKey("AppUser")]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("quiz_text", TypeName = "nvarchar(255)")]
    public string QuizText { get; set; }

    [Required]
    [Column("answer", TypeName = "nvarchar(max)")]
    public string Answer { get; set; }

    [Column("image", TypeName = "varbinary(max)")]
    public string? Image { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<DeckCard> DeckCards { get; set; }
  }
}
