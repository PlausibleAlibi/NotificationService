using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Api.Services;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(
        JwtService jwtService,
        ILogger<AuthController> logger,
        IConfiguration configuration)
    {
        _jwtService = jwtService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Login and get JWT token
    /// </summary>
    /// <remarks>
    /// For demo purposes, accepts username "admin" with password "admin123".
    /// In production, use proper user authentication with hashed passwords.
    /// </remarks>
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        // Simple hardcoded authentication for demo purposes
        // In production, validate against database with hashed passwords
        var validUsername = _configuration["Auth:DemoUsername"] ?? "admin";
        var validPassword = _configuration["Auth:DemoPassword"] ?? "admin123";

        if (request.Username != validUsername || request.Password != validPassword)
        {
            _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = _jwtService.GenerateToken(request.Username);
        var expiresAt = _jwtService.GetTokenExpiry();

        _logger.LogInformation("User {Username} logged in successfully", request.Username);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = request.Username,
            ExpiresAt = expiresAt
        });
    }
}
