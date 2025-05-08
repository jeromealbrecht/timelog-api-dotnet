using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeLog.Models;
using TimeLog.Services;

namespace TimeLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _userService.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest("Un utilisateur avec cet email existe déjà.");
            }

            await _userService.CreateAsync(user);
            var token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            if (!_userService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 