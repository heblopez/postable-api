using System.ComponentModel.DataAnnotations;

namespace PostableApi.Models.Dtos;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 8 characters long")]
    public string? Password { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Role { get; set; } = "user";
}