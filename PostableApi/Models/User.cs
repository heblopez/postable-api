using System.ComponentModel.DataAnnotations;

namespace PostableApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [Required]
    public string? Role { get; set; } = "user";
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}