using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostableApi.Models;

public class Like
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Post")]
    public int PostId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post? Post { get; set; }
    public User? User { get; set; }
}