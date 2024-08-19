using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_API.Models
{
  [Table("user_auth")]
  public class UserAuth
  {
    [Key, ForeignKey("AppUser")]
    [Column("user_id", TypeName = "uniqueidentifier")]
    public Guid UserId { get; set; }

    [Required]
    [Column("username", TypeName = "nvarchar(16)")]
    public string Username { get; set; }

    [Required]
    [Column("user_pwd", TypeName = "nvarchar(128)")]
    public string UserPwd { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation property
    public virtual AppUser AppUser { get; set; }
  }
}
