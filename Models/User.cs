using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Models
{
  [Table("app_user")]
  public class AppUser
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id", TypeName = "uniqueidentifier")]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [Required]
    [Column("full_name", TypeName = "nvarchar(100)")]
    public string FullName { get; set; }

    [Required]
    [Column("email", TypeName = "nvarchar(255)")]
    public string Email { get; set; }

    [Required]
    [Column("email_confirmed")]
    public bool IsEmailConfirmed { get; set; } = false;

    [Column("date_of_birth", TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [Column("registration_date", TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    [Column("last_login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [Column("is_active", TypeName = "bit")]
    public bool IsActive { get; set; } = true;
  }
}
