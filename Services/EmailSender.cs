namespace ECommerceAPI.Services
{
    public class EmailSender: IEmailSender
    {
        public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
        {
            // SMTP logic here

            Console.WriteLine($"Email sent to {to}");

            await Task.CompletedTask;
        }
    }
}
