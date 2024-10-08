using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PostableApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string Password { get; set; }
    
    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? Role { get; set; } = "user";
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [JsonIgnore]
    public List<Post>? Posts { get; set; }
    
    [JsonIgnore]
    public List<Like>? Likes { get; set; }
}