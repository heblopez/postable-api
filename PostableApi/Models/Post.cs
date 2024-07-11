using System.ComponentModel.DataAnnotations;

namespace PostableApi.Models;

public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(480)]
    public string? Content { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    
    public List<Like>? Likes { get; set; } 
}