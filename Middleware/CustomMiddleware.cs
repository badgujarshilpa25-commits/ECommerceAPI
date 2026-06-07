namespace ECommerceAPI.Middleware
{
    public class CustomMiddleware 
    {
        private readonly RequestDelegate _requestDelegate;
        public CustomMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Message = "An unexpected error occurred.", error = ex.Message });
            }
        }
    }
}
