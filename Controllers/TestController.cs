using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IGreetingService _greetingService;
        private readonly IConfiguration _config;
        public TestController(IGreetingService greetingService, IConfiguration config)
        {
            _greetingService = greetingService;
            _config = config;
        }

        [HttpGet()]
        public IActionResult Greet()
        {
            var message = _greetingService.GetGreeting();
            return Ok(message);
        }
    }
}