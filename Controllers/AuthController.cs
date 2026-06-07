using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        ILogger<AuthController> _logger;
        IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to login user.");
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid login data provided.");
                    return BadRequest(ModelState);
                }
                else
                {
                    _logger.LogInformation("Login data is valid.");
                    // For demonstration purposes, we will return a dummy token
                    var token = _authService.Login(loginDto);
                    return Ok(new { Token = token });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Something went wrong");
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Attempting to register user.");
            try
            {
                // Implementation for user registration
                // Create a new user and return a success message
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid registration data provided.");
                    return BadRequest(ModelState);
                }
                else
                {
                    _logger.LogInformation("Registration data is valid.");
                    var token = _authService.Register(registerDto);
                    return Ok(new { Token = token });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Something Went Wrong");
                return BadRequest();
            }
        }
    }
}
