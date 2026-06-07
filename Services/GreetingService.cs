namespace EcommerceAPI.Services
{
    public class GreetingService : IGreetingService
    {
        public string GetGreeting()
        {
            return "Hello from DI.";
        }
    }
}