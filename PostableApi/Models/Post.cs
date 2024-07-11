using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public User? User { get; set; }
    
    [JsonIgnore]
    public List<Like>? Likes { get; set; } 
}