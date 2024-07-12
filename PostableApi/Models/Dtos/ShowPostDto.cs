namespace PostableApi.Models.Dtos;

public class ShowPostDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Username { get; set; }
    public int LikesCount { get; set; }
}