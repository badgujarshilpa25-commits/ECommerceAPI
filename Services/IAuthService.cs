using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IAuthService
    {
        public Task<string> Register(RegisterDto registerDto);
        public Task<string> Login(LoginDto loginDto);
    }
}
