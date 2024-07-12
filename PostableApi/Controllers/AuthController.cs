using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostableApi.Data;
using PostableApi.Models;

namespace PostableApi.Controllers;

[Route("/")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    
    public AuthController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
        {
            return BadRequest(new {message = "Username already exists!"});
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Password = hashedPassword;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new {message = "User registered successfully!"});
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == userLogin.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
        {
            return Unauthorized(new {message = "Invalid username, please try again!"});
        }

        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
        {
            return Unauthorized(new {message = "Invalid password, please try again!"});
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Username!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role!)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken
        (
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new {token = tokenString, message = "User logged in successfully!"});
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new {message = "User logged out successfully!"});
    }
    
    public class UserLogin
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}