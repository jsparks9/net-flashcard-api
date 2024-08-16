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

    [Column("quiz_text", TypeName = "nvarchar(255)")]
    public string QuizText { get; set; }

    [Required]
    [Column("answers", TypeName = "nvarchar(max)")]
    public string Answers { get; set; }

    [Column("image", TypeName = "varbinary(max)")]
    public byte[] Image { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
  }
}
