using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostableApi.Data;
using PostableApi.Models;
using PostableApi.Models.Dtos;

namespace PostableApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult<List<PostGetDto>> GetPosts([FromQuery] string? username, [FromQuery] string? orderBy, [FromQuery] string? order)
    {
        IQueryable<Post> allPosts = _context.Posts.Include(p => p.User).Include(p => p.Likes);
        
        if (!string.IsNullOrEmpty(username))
        {
            allPosts = allPosts.Where(p => p.User!.Username == username);
        }
        
        IOrderedQueryable<Post> sortedPosts;

        if (orderBy == "likesCount")
        {
            sortedPosts = order != null && order.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? allPosts.OrderByDescending(p => p.Likes!.Count) : allPosts.OrderBy(p => p.Likes!.Count);
        } 
        else
        {
            sortedPosts = order != null && order.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? allPosts.OrderByDescending(p => p.CreatedAt) : allPosts.OrderBy(p => p.CreatedAt);
        }

        var postsResponse = sortedPosts.Select(p => new PostGetDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Username = p.User!.Username,
            LikesCount = p.Likes!.Count
        }).ToList();
        
        return Ok(postsResponse);
    }
    
    [HttpPost]
    public ActionResult<Post> PostPost([FromBody] Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();

        return Ok(post);
    }
}