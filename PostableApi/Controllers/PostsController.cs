using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    public ActionResult<List<ShowPostDto>> GetPosts([FromQuery] string? username, [FromQuery] string? orderBy, [FromQuery] string? order)
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

        var postsResponse = sortedPosts.Select(p => new ShowPostDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Username = p.User!.Username,
            LikesCount = p.Likes!.Count
        }).ToList();
        
        return Ok(postsResponse);
    }

    [HttpGet("{id}")]
    public ActionResult<ShowPostDto> GetPostById(int id)
    {
        var post = _context.Posts
            .Include(p => p.User)
            .Include(p => p.Likes)
            .FirstOrDefault(p => p.Id == id);
        
        if (post == null)
        {
            return NotFound();
        }
        
        var postResponse = new ShowPostDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Username = post.User!.Username,
            LikesCount = post.Likes!.Count
        };

        return Ok(postResponse);
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ShowPostDto), StatusCodes.Status201Created)]
    public CreatedAtActionResult CreatePost([FromBody] CreatePostDto newCreatePost)
    {
        var post = new Post
        {
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            Content = newCreatePost.Content
        };
        
        _context.Posts.Add(post);
        _context.SaveChanges();
        
        var postResponse = new ShowPostDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Username = User.FindFirstValue(JwtRegisteredClaimNames.PreferredUsername),
            LikesCount = 0
        };
        
        return CreatedAtAction(nameof(GetPostById), new {id = postResponse.Id}, postResponse);
    }
    
    [Authorize]
    [HttpPatch("{id}")]
    public ActionResult<ShowPostDto> UpdatePost([FromBody] CreatePostDto postToUpdate, int id)
    {
        var post = _context.Posts
            .Include(p => p.User).Include(post => post.Likes!)
            .FirstOrDefault(p => p.Id == id);
        
        if (post == null)
        {
            return NotFound();
        }
        
        if (post.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))
        {
            return Unauthorized("You are not authorized to edit this post. Only the author can edit their posts!");
        }
        
        post.Content = postToUpdate.Content;
        _context.SaveChanges();
        
        var postResponse = new ShowPostDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Username = User.FindFirstValue(JwtRegisteredClaimNames.PreferredUsername),
            LikesCount = post.Likes!.Count
        };
        
        return Ok(postResponse);
    }
}