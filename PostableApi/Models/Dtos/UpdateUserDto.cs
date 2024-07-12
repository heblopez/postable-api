using System.ComponentModel.DataAnnotations;

namespace PostableApi.Models.Dtos;

public class UpdateUserDto
{
    [EmailAddress]
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}