using EcommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        IConfiguration _configuration;
        ILogger<AuthService> logger;
        public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            this._configuration = configuration;
            this.logger = logger;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == loginDto.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid email or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    throw new InvalidOperationException("Invalid email or password.");
                }

                var token = GenerateJwt(user);

                return token;
            }
            catch(Exception ex)
            {
                logger.LogError("Token generation failed: " + ex.Message);
                return "";
            }
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(x => x.Email == registerDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("User with this email already exists.");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    throw new ArgumentException("Password is required.");
                }

                // Hash the password using BCrypt
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                var token = GenerateJwt(user);

                return token;
            }
            catch(Exception ex)
            {
                logger.LogError("Token generation failed: "+ ex.Message);
                return "";
            }
        }

        private string GenerateJwt(User user)
        {
            try
            {
                var jwtSection = _configuration.GetSection("Jwt");
                var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key missing");
                var issuer = jwtSection["Issuer"];
                var audience = jwtSection["Audience"];
                var durationMinutes = int.TryParse(jwtSection["DurationInMinutes"], out var d) ? d : 60;

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("name", user.Name ?? string.Empty)
            };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(durationMinutes),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                logger.LogError("Token generation failed: "+ ex.Message);
                return "";
            }
        }
    }
}
