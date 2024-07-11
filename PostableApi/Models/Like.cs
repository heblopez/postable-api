using System.ComponentModel.DataAnnotations;

namespace PostableApi.Models;

public class Like
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PostId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post? Post { get; set; }
    
    public User? User { get; set; }
}